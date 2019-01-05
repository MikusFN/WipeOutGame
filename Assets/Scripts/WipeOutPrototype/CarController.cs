using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration;
    public float rotationRate;
    public float rotationAngle;
    public float turnSpeed;
    public float smothTime;

    float rotationVelocity;
    //float groundAngVelocity;
    public new Rigidbody rigidbody;
    //public GameObject track;
    public BezierSpline spline;
    public SplineWalker walker;
    private Vector3 lastPosition, lastFoward, splineDirection;

    public void Start()
    {
        Vector3 pos = spline.GetPointInSpline(0.01f);
        pos.y = pos.y + 0.3f;
        //pos.z = pos.z + 5;
        //pos.x = pos.x + 5;

        transform.position = pos;
        transform.LookAt(pos + spline.GetDirection(0.001f));

    }    

    public void Move(float sterring, float throttle, bool autoPilot)
    {
        Vector3 fowardForce = Vector3.zero;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1f))//Se esta a tres unidades do chao (colocar numa funcao)
        {
            rigidbody.drag = 5f;// Para calcular a resitencia do ar no objecto que tem o rigidBody (decellarate)
                                //Turn into a function that computes add force based on input, gets a vector an adds force
            fowardForce = transform.forward * acceleration *throttle;
            fowardForce = fowardForce * Time.fixedDeltaTime * rigidbody.mass;
            rigidbody.AddForce(fowardForce, ForceMode.Force);
            
        }
        else
        {
            rigidbody.drag = 0;
            if (Input.GetKey(KeyCode.F))
            {
                transform.Rotate(transform.forward.normalized, 180);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                rigidbody.velocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
                transform.forward = lastFoward;
                //transform.forward = track.GetComponentInChildren<TilesManager>().currentTile.transform.forward;
                transform.position = lastPosition;//track.GetComponentInChildren<TilesManager>().currentTile.transform.position - Vector3.forward + Vector3.up * 5;
            }
        }

        Vector3 turnForce = transform.up * rotationRate;

        //if(Input.GetKeyDown(KeyCode.I))
        //Debug.Log(spline.GetDirection(transform.position));

        if ((throttle) >= 0)
            turnForce = turnForce * sterring;
        else
        {
            turnForce = turnForce * -sterring;
        }
        splineDirection = spline.GetDirection(transform.position);

        //if (transform.rotation.x < 10 && transform.rotation.x > -10)
        //Debug.Log(transform.rotation.x < -10.0f);
        if (autoPilot)
        {
            turnForce = Vector3.zero;
            transform.LookAt(transform.position + splineDirection);
        }
        turnForce = turnForce * Time.fixedDeltaTime * rigidbody.mass;
        rigidbody.AddTorque(turnForce, ForceMode.Force);


        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, sterring * -rotationAngle, ref rotationVelocity, smothTime, turnSpeed, Time.fixedDeltaTime);

        transform.eulerAngles = newRotation;
        if (Physics.Raycast(transform.position, -transform.up, 1f) && transform.rotation.z < 45)
        {
            lastPosition = transform.position;
            lastFoward = transform.forward;
            //Debug.DrawRay(transform.position, -transform.up, Color.black);
        }
    }

    public void ActivateThrusters()
    {

        if ( GetComponent<ThrusterController>().strenght < GetComponent<ThrusterController>().maxStreghtStart && GetComponent<ThrusterController>().strenght == 0)
        {
            //GetComponent<ThrusterController>().distancePercent = 1;
            GetComponent<ThrusterController>().strenght = GetComponent<ThrusterController>().maxStreghtStart;
            //GetComponent<ThrusterController>().distanceMax = 2f;

        }
        else if (GetComponent<ThrusterController>().strenght > GetComponent<ThrusterController>().maxStrenght)
        {
            GetComponent<ThrusterController>().strenght -= 10f;

        }
    }
}



