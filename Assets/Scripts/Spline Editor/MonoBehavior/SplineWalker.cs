using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{

    public BezierSpline spline;
    public float duration;
    private float progress;
    private Vector3 positionSpline;
    public bool lookAhead;
    public SplineWalkerMode mode;
    public GameObject prefab;
    Vector3 lastPosition;
    public enum SplineWalkerMode
    {
        //Modos de circulação
        Loop,
        Once,
        PingPong
    }

    private void Start()
    {
        lastPosition = transform.position;
        transform.position = spline.GetPointCubicInCurve(0);
        lookAhead = true;
    }
    private void Update()
    {
        LineWalker(ref progress, duration,spline, lastPosition);
        lastPosition = transform.position;
    }

    //Coloca esta transform local position na spline que vem da spline
    private void LineWalker(ref float p, float walkTime, BezierSpline spline, Vector3 lastPosition)
    {
        if (lookAhead)
        {
            //A walk é ao longo do tempo
            p += Time.deltaTime / walkTime;
            //Bound para o clamp de 0 a 1 da spline e os modos do objecto
            if (p > 1f)
            {
                if (mode == SplineWalkerMode.Once)
                {
                    p = 1f;
                }
                else if (mode == SplineWalkerMode.Loop)
                {
                    p -= 1f;
                }
                else
                {
                    p = 2f - p;
                    lookAhead = false;
                }
            }
        }
        else
        {
            p -= Time.deltaTime / walkTime;
            if (p < 0f)
            {
                p = -p;
                lookAhead = true;
            }
        }

        //Atribuição da posiçao
        positionSpline = spline.GetPointInSpline(p);
        transform.position = Vector3.Lerp( positionSpline, lastPosition, 0.9f);

        if (lookAhead)
        {
            transform.LookAt(positionSpline + spline.GetDirection(p));
        }
        //if (p != 1)
        //    Instantiate(prefab, spline.GetPointInSpline(p), Quaternion.identity); //= spline.GetPointInSpline(p);
        //Rigidbody rb = GetComponent<Rigidbody>();

        //if (rb)
        //    rb.velocity += spline.GetDirectionCubic(p);


    }
}
