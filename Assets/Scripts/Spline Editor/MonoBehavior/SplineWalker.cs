using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{

    public BezierSpline spline;
    [SerializeField]
    public float duration, maxDuration, minDuration;
    [SerializeField]
    int visionDistance;
    private float progress;
    private Vector3 positionSpline;
    public bool lookAhead, goingForward = true;
    public SplineWalkerMode mode;
    Vector3 lastPosition;
    public GameObject player;
    public enum SplineWalkerMode
    {
        //Modos de circulação
        Loop,
        Once,
        PingPong
    }

    private void Start()
    {
        transform.position = player.transform.position + transform.forward * 5;
        lastPosition = transform.position;
        lookAhead = true;
    }
    private void LateUpdate()
    {

        //if ((player.transform.position - transform.position).magnitude <20 )
        //{
        //    duration -= (player.transform.position - transform.position).magnitude * 0.1f;
        //}
        //if ((duration < 80|| (player.transform.position - transform.position).magnitude >40))
        //    duration += (player.transform.position - transform.position).magnitude * 0.1f;

        Debug.Log(Vector3.Dot((player.transform.position - transform.position), transform.forward));

        //Debug.Log(duration);
        if ((player.transform.position - transform.position).magnitude > visionDistance)
        {
            if (Vector3.Dot((player.transform.position - transform.position), transform.forward) < 0f)
            {
                if (duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) > minDuration && duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) < maxDuration)
                    duration -= Vector3.Dot((player.transform.position - transform.position), transform.forward);

            }
            else if (Vector3.Dot((player.transform.position - transform.position), transform.forward) > 0f)
            {
                if (duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) > minDuration && duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) < maxDuration)
                    duration -= Vector3.Dot((player.transform.position - transform.position), transform.forward);
            }
        }
        else
        {
            if (Vector3.Dot((player.transform.position - transform.position), transform.forward) < 0f)
            {
                if (duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) > minDuration && duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) < maxDuration)
                    duration -= Vector3.Dot((player.transform.position - transform.position), transform.forward)*(1/ (player.transform.position - transform.position).magnitude);

            }
            else if (Vector3.Dot((player.transform.position - transform.position), transform.forward) > 0f)
            {
                if (duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) > minDuration && duration - Vector3.Dot((player.transform.position - transform.position), transform.forward) < maxDuration)
                    duration -= Vector3.Dot((player.transform.position - transform.position), transform.forward)*(1 / (player.transform.position - transform.position).magnitude);
            }
        }

        LineWalker(ref progress, duration, spline, lastPosition);
        lastPosition = transform.position;
    }

    //Coloca esta transform local position na spline que vem da spline
    private void LineWalker(ref float p, float walkTime, BezierSpline spline, Vector3 lastPosition)
    {
        if (goingForward)
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
                    goingForward = false;
                }
            }
        }
        else
        {
            p -= Time.deltaTime / walkTime;
            if (p < 0f)
            {
                p = -p;
                goingForward = true;
            }
        }

        //Atribuição da posiçao
        positionSpline = spline.GetPointInSpline(p);
        transform.position = Vector3.Lerp(positionSpline, lastPosition, 0.9f);

        if (lookAhead)
            transform.LookAt(positionSpline + spline.GetDirection(p));

        //if (p != 1)
        //    Instantiate(prefab, spline.GetPointInSpline(p), Quaternion.identity); //= spline.GetPointInSpline(p);
        //Rigidbody rb = GetComponent<Rigidbody>();

        //if (rb)
        //    rb.velocity += spline.GetDirectionCubic(p);


    }
}
