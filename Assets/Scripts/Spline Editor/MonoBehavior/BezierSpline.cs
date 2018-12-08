using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    #region Fields
    public Vector3[] points;
    private int curveCount;

    #endregion Fields

    #region Properties
    public Vector3[] Points
    { get { return points; } }

    public int CurveCount { get { return (points.Length - 1 )/ 3; } } // Uma curve a cada 3 pontos.

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
            lastPoint.x = lastPoint.x + 1;
            //Coloca o novo ponto no array
            points[points.Length - i] = lastPoint;
        }
    }

    //Funçao que permite obter o ponto na curva entre o primeiro
    //e o ultimo elementos do array de pontos de acordo com um valor t
    //Com o elemento intermedio a "puxar" a curva para si
    public Vector3 GetPoint(float t)
    {
        //Indexador de pontos na curva
        int i=-1;

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

    public Vector3 GetPointCubic(float t)
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
    #endregion Methods
}
