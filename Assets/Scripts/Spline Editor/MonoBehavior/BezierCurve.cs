using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    #region Fields
    public Vector3[] points;
    #endregion Fields

    #region Properties
    public Vector3[] Points
    { get { return points; } }

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

    //Funçao que permite obter o ponto na curva entre o primeiro
    //e o ultimo elementos do array de pontos de acordo com um valor t
    //Com o elemento intermedio a "puxar" a curva para si
    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
    }

    public Vector3 GetPointCubic(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], t)) - transform.position;
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
