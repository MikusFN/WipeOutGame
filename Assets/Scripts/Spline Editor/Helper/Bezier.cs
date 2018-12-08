using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{

    #region Fields

    #endregion Fields

    #region Properties

    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods

    //Obter o ponto na curva
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        //Normalizar t para valores entre 0 e 1.
        t = Mathf.Clamp01(t);
        float oneMinusT = 1 - t;
        return Mathf.Pow(oneMinusT, 2) * p0 + 2 * oneMinusT * t * p1 + Mathf.Pow(t, 2) * p2; //(1-t)2 * p0 + 2 * (1 - t) *t *p1 +t2 *p2 -> Equação de um polinomio de segundo grau (como computar dois lerps)
                                                                                             // return Vector3.Lerp( Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t),t) ;
    }
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
        oneMinusT * oneMinusT * oneMinusT * p0 +
        3f * oneMinusT * oneMinusT * t * p1 +
        3f * oneMinusT * t * t * p2 +
        t * t * t * p3;
    }
    //Primeira derivada da curva no ponto t.
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1 - t;
        return 2 * (oneMinusT) * (p1 - p0) + 2 * t * (p2 - p1);
    }
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
        3f * oneMinusT * oneMinusT * (p1 - p0) +
        6f * oneMinusT * t * (p2 - p1) +
        3f * t * t * (p3 - p2);
    }
    public static Vector3 GetSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return // 3 * (1 - t)2 * (p1 - p0) + 6 * (1-t) * t * (p2 - p1) * 3 * t2 * (p3 - p2)
        6f * oneMinusT * (p1 - p0) +
        6f * oneMinusT * t * (p2 - p1) +
        3f * t * t * (p3 - p2);
    }
    #endregion Methods

}
