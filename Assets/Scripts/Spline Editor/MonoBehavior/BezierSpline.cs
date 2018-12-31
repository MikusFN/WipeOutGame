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
    [SerializeField]// So unity can save our points
    private List<Vector3> pointsMesh, pointsRightRail, pointsLeftRail;
    [SerializeField]
    private CONTROLPOINTSMODE[] modes;
    [SerializeField]
    private bool inLoop;
    private int curveCount;
    public int width, height;
    #endregion Fields

    #region Properties
    public Vector3[] Points
    { get { return points; } }
    public int PointsCount { get { return points.Length; } }
    public Vector3 GetterPoint(int index) { return points[index]; }
    public Vector3 GetterPointMesh(int index) { return pointsMesh[index]; }

    public void SetPoint(int index, Vector3 pointValue)
    {
        MovePointsAlong(index, pointValue);
        points[index] = pointValue;
        EnforceMode(index);
    }
    public int CurveCount { get { return (points.Length - 1) / 3; } } // Uma curve a cada 3 pontos.
    public bool InLoop
    {
        get { return inLoop; }
        set
        {
            inLoop = value;
            if (value == true)
            {
                // No caso de estar em loop 
                //então o ultimo ponto fica com o mode do primeiro 
                modes[modes.Length - 1] = modes[0];
                //e a sua posição tambem será a mesma.
                SetPoint(0, points[0]);
            }
        }
    }
    public List<Vector3> PointsMesh
    {
        get
        {
            return pointsMesh;
        }
    }

    public List<Vector3> PointsRightRail
    {
        get
        {
            return pointsRightRail;
        }

    }

    public List<Vector3> PointsLeftRail
    {
        get
        {
            return pointsLeftRail;
        }
    }

    public CONTROLPOINTSMODE GetControlPointMode(int index) { return modes[(index + 1) / 3]; }
    public void SetControlPointMode(int index, CONTROLPOINTSMODE mode)
    {
        //Uma spline com 7 pontos (0, 1, 2, 3, 4, 5, 6) tem uma sequencia de modos como esta (0, 0, 1, 1, 1, 2, 2)
        //Pontos das bounds partilham o mesmo modo e os tres do centro tambem o têm o mesmo modo
        modes[(index + 1) / 3] = mode;
        int modeIndex = (index + 1) / 3;
        if (inLoop)
        {
            //No caso de estar em loop entao os modes sao iguais no inicio e fim sendo que se for no inicio aplica-se no fim e vice versa
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }

    public void UpdatePointsNaSpline()
    {
        foreach (Transform item in GetComponentInChildren<ItemsInSpline>().items)
        {
            DestroyImmediate(item.gameObject, true);
        }
    }

    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods

    private void Awake()
    {
        Vector3 initialPosition= GetPointInSpline(1);
        initialPosition.x = -initialPosition.x;
        GetComponent<Transform>().position = initialPosition;
    }

    public void Reset()// Special unity method que é chamada quando usamos o reset button no inspector
    {
       
        points = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(50,0,50),
            new Vector3(100,0,0),
            new Vector3(150,0,-50)

        };
        //Apensa precisasmos de modificar os pontos de controlo (primeiro e ultimo)
        modes = new CONTROLPOINTSMODE[] {
        CONTROLPOINTSMODE.MIRRORED,
        CONTROLPOINTSMODE.MIRRORED
        };
        for (int i = 0; i < 10; i++)
        {
            AddCurve();
        }
        //points[points.Length - 1].y = points[points.Length-1].y - 50;
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
            lastPoint.x = lastPoint.x + 50;

            if (i % 2 == 0)
                lastPoint.z = lastPoint.z + 50;
            else
                lastPoint.z = 0;

            //Coloca o novo ponto no array
            points[points.Length - i] = lastPoint;

        }
        //Colocar um  novo modo para o novo ponto de controlo só precisa de um novo modo porque cada nova curva so pode ter um modo diferente os ultimo dois pontos
        Array.Resize(ref modes, modes.Length + 1);
        //Os dois novos pontos( sem ser o reutilizado para ligar à anterior) é que podem ter um modo diferente mas partilhado entre os dois ultimos
        modes[modes.Length - 1] = modes[modes.Length - 2];
        //Aplicar o mode do ultimo ponto da curva, antes desta nova ser adicionada, aos novos pontos.
        EnforceMode(modes.Length - 4);

        //Quando em loop o ultimo ponto passa a ser como o primeiro, tanto na localização como no mode
        if (inLoop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
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

    //public Vector3 GetPointCubicInCurve(float t)
    //{
    //    return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    //}

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
    //public Vector3 GetVelocityCubic(float t)
    //{
    //    return transform.TransformPoint(
    //    Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
    //}

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    //public Vector3 GetDirectionCubic(float t)
    //{
    //    return GetVelocityCubic(t).normalized;
    //}

    //Aplica o mode correspondente ao index que recebe
    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;

        CONTROLPOINTSMODE mode = modes[modeIndex];
        //Verifica quando nao deve aplicar o novo mode (Free, nao esta em loop, no inicio ou fim da curva)
        if (mode == CONTROLPOINTSMODE.FREE || !inLoop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        { return; }

        // Seletor de indices onde o que escolhemos fica sempre fixo 

        // Os Pontos sao definidos a cada 3
        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;

        //index selector em que se for inferior 
        //ao ponto medio de uma curva entra neste bloco
        if (index <= middleIndex)
        {
            // O ponto anterior fica marcado como fixo
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            // E força o ponto a seguir 
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else // Caso o ponto esteja à frente
        {
            // O ponto fixo é o proximo 
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            // E força-se o anterior
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }
        // Vamos buscar o ponto medio
        Vector3 middle = points[middleIndex];

        // Calculamos o vector entre o fixo e o escolhido
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == CONTROLPOINTSMODE.ALIGN)
        {
            //No caso de align aplicamos o valor do vector mas com a distancia que ja tinha
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        //No caso de ser um reflexo aplca-se o mesmo valor do ponto medio ao ponto vizinho nao escolhido
        points[enforcedIndex] = middle + enforcedTangent;
    }

    //Move o pontos visinhos com o medio
    private void MovePointsAlong(int index, Vector3 pointValue)
    {
        if (index % 3 == 0) //Se for um ponto na curva e nao de controlo
        {
            Vector3 delta = pointValue - points[index]; //Guarda-se o valor de movimento

            //Verificação dos pontos a mover em loop
            if (inLoop)
            {
                if (index == 0)// Moviemnto do primeiro
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = pointValue;
                }
                else if (index == points.Length - 1) // Moviemnto do ultimo
                {
                    points[0] = pointValue;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else //Qualquer outro ponto
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            //if (index > 0) // Caso seja superior a zero move o que esta a seguir
            //{
            //    points[index - 1] += delta;
            //}
            //if (index + 1 < points.Length) // Caso seja inferioe ao maximo move o que esta a antes
            //{
            //    points[index + 1] += delta;
            //}
        }
    }
    //public void GiveOnlyPoints()
    //{
    //    List<Vector3> pointsInCurve = new List<Vector3>();
    //    for (int i = 0; i < points.Length; i++)
    //    {
    //        if (i % 3 == 0)
    //        {
    //            pointsInCurve.Add(points[i]);
    //        }
    //    }
    //    DefineMeshPoints(pointsInCurve);
    //}
    public void DefineMeshPoints(List<Vector3> pointsInCurve)
    {
        //Vector3 right = points[0] + Vector3.right, left = points[0] + Vector3.left, up = points[0] + Vector3.up;
        Vector3 foward;
        List<Vector3> up = new List<Vector3>();
        for (int i = 0; i < pointsInCurve.Count; i++)
        {
            foward = Vector3.zero;

            if (i > 0 || inLoop)
            {
                foward += pointsInCurve[i] - pointsInCurve[(i - 1 + pointsInCurve.Count) % pointsInCurve.Count];
            }
            if (i < pointsInCurve.Count - 1 || inLoop)
            {
                foward += pointsInCurve[(i + 1) % pointsInCurve.Count] - pointsInCurve[i];
            }
            foward.Normalize();
            Vector3 left = new Vector3(-foward.z, foward.y, foward.x);
            up.Add(Vector3.Cross(foward, left));
            up.Add(Vector3.Cross(foward, -left));
            pointsMesh.Add(pointsInCurve[i] + left * width * 0.5f);
            pointsMesh.Add(pointsInCurve[i] - left * width * 0.5f);


            //right = new Vector3((points[i] - points[i + 3]).z, (points[i] - points[i + 3]).y,(points[i]-points[i+3]).x);
            //pointsMesh.Add(right);
            //left = Vector3.Cross(points[i + 3] - points[i],up).normalized;
            //pointsMesh.Add(left);

        }
        for (int i = 0; i < pointsMesh.Count; i++)
        {
            if (i % 2 == 0)
            {
                pointsLeftRail.Add(pointsMesh[i] - up[i] * height * 0.5f);
                pointsLeftRail.Add(pointsMesh[i] + up[i] * height * 0.5f);

            }
            else
            {
                pointsRightRail.Add(pointsMesh[i] - up[i] * height * 0.5f);
                pointsRightRail.Add(pointsMesh[i] + up[i] * height * 0.5f);
            }
        }
        //PointsLeftRail[PointsLeftRail.Count - 1] = PointsLeftRail[1];
        //PointsRightRail[PointsRightRail.Count - 1] = PointsRightRail[1];
        //PointsMesh[pointsMesh.Count - 1] = PointsMesh[1];
        //PointsLeftRail[PointsLeftRail.Count - 2] = PointsLeftRail[0];
        //PointsRightRail[PointsRightRail.Count - 2] = PointsRightRail[0];
        //PointsMesh[pointsMesh.Count - 2] = PointsMesh[0];
        //pointsLeftRail.Add(pointsLeftRail[1]);
        //pointsRightRail.Add(pointsRightRail[1]);
        //pointsMesh.Add(pointsMesh[1]);
        //pointsLeftRail.Add(pointsLeftRail[0]);
        //pointsRightRail.Add(pointsRightRail[0]);
        //pointsMesh.Add(pointsMesh[0]);

    }

}
#endregion Methods


