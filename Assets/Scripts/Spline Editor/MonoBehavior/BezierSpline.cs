using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    #region Fields
    [SerializeField]// So unity can save our points
    private Vector3[] points;
    [SerializeField]
    private CONTROLPOINTSMODE[] modes;

    private int curveCount;
    #endregion Fields

    #region Properties
    public Vector3[] Points
    { get { return points; } }
    public int PointsCount { get { return points.Length; } }
    public Vector3 GetterPoint(int index) { return points[index]; }
    public void SetPoint(int index, Vector3 pointValue)
    {
        points[index] = pointValue;
        EnforceMode(index);
    }
    public int CurveCount { get { return (points.Length - 1) / 3; } } // Uma curve a cada 3 pontos.
    public CONTROLPOINTSMODE GetControlPointMode(int index) { return modes[(index + 1) / 3]; }
    public void SetControlPointMode(int index, CONTROLPOINTSMODE mode)
    {
        modes[(index + 1) / 3] = mode;
        EnforceMode(index);
    }
    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods

    public void Reset()// Special unity method que é chamada quando usamos o reset button no inspector
    {
        points = new Vector3[] {
            new Vector3(-5,0,0),
            new Vector3(-3,0,2),
            new Vector3(3,0,0),
            new Vector3(5,0,2)
        };
        //Apensa precisasmos de modificar os pontos de controlo (primeiro e ultimo)
        modes = new CONTROLPOINTSMODE[] {
        CONTROLPOINTSMODE.FREE,
        CONTROLPOINTSMODE.FREE
        };
    }

    public void AddCurve()
    {
        //Guardar o ultimo ponto
        Vector3 lastPoint = points[points.Length - 1];
        //Altera o tamanho do array de pontos para albergar 3 novos pontos
        Array.Resize(ref points, points.Length + 3);

        for (int i = 3; i > 0; i--)
        {
            // Avança o ultimo ponto
            lastPoint.x = lastPoint.x + 5;

            if (i % 2 == 0)
                lastPoint.z = lastPoint.z + 5;
            else
                lastPoint.z = 0;

            //Coloca o novo ponto no array
            points[points.Length - i] = lastPoint;
        }
        //Colocar um  novo modo para o novo ponto de controlo
        Array.Resize(ref modes, modes.Length + 1);
        //O tipo de ponto tem que ser igual ao ultimo para manter a derivada.
        modes[modes.Length - 1] = modes[modes.Length - 2];
    }

    //Funçao que permite obter o ponto na curva entre o primeiro
    //e o ultimo elementos do array de pontos de acordo com um valor t
    //Com o elemento intermedio a "puxar" a curva para si
    public Vector3 GetPointInSpline(float t)
    {
        //Indexador de pontos na curva
        int i = -1;

        //Valor maximo da curva (Quando o valor de f é superior a 1)
        if (t >= 1f)
        {
            //Fica a 1
            t = 1f;
            //No fim da curva para obter os ultimos pontos
            i = points.Length - 4;
        }
        else
        {
            //Precaver -se de valor negativos e  multiplica-se para saber em que parte da curva se esta. (0.7*8=5.6) -> 5 curva a 60% do caminho para 6
            t = Mathf.Clamp01(t) * CurveCount;
            //Guarda o valor da curva
            i = (int)t;
            //Guarda-se a percentagem de percurso da ultima curva
            t -= i;
            //Colocar o i de acordo com os pontos a indexar para estar na curva
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetPointCubicInCurve(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        //Indexador de pontos na curva
        int i;

        //Valor maximo da curva (Quando o valor de f é superior a 1)
        if (t >= 1f)
        {
            //Fica a 1
            t = 1f;
            //No fim da curva para obter os ultimos pontos
            i = points.Length - 4;
        }
        else
        {
            //Precaver -se de valor negativos e  multiplica-se para saber em que parte da curva se esta. (0.7*8=5.6) -> 5 curva a 60% do caminho para 6
            t = Mathf.Clamp01(t) * CurveCount;
            //Guarda o valor da curva
            i = (int)t;
            //Guarda-se a percentagem de percurso da ultima curva
            t -= i;
            //Colocar o i de acordo com os pontos a indexar para estar na curva
            i *= 3;
        }
        //Obtem-se os tres pontos para as velocidades
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }
    public Vector3 GetVelocityCubic(float t)
    {
        return transform.TransformPoint(
        Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public Vector3 GetDirectionCubic(float t)
    {
        return GetVelocityCubic(t).normalized;
    }
    //Aplica o mode correspondente ao index que recebe
    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;

        CONTROLPOINTSMODE mode = modes[modeIndex];
        //Verifica quando nao deve aplicar o novo mode (Free, no inicio ou fim da curva)
        if (mode == CONTROLPOINTSMODE.FREE || modeIndex == 0 || modeIndex == modes.Length - 1)
        { return; }

        /*Now which point should we adjust? When we change a point's mode, it is either a point in
          between curves or one of its neighbors. When we have the middle point selected, we can just
          keep the previous point fixed and enforce the constraints on the point on the opposite side. If we
          have one of the other points selected, we should keep that one fixed and adjust its opposite. That
          way our selected point always stays where it is. So let's define the indices for these points.*/

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        //index selector
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            enforcedIndex = middleIndex + 1;
        }
        else
        {
            fixedIndex = middleIndex + 1;
            enforcedIndex = middleIndex - 1;
        }
        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == CONTROLPOINTSMODE.ALIGN)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }
    #endregion Methods
}
