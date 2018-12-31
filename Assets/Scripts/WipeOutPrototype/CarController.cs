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
    float groundAngVelocity;
    public new Rigidbody rigidbody;
    public GameObject track;
    public BezierSpline spline;
    private Vector3 lastPosition, lastFoward;

    public void Start()
    {
        Vector3 pos = spline.GetPointInSpline(0.01f);
        pos.y = pos.y + 1;
        //pos.z = pos.z + 5;
        //pos.x = pos.x + 5;

        transform.position = pos;
        transform.LookAt(pos + spline.GetDirection(0.01f));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 fowardForce = Vector3.zero;
        if (Physics.Raycast(transform.position, -transform.up, 1f))//Se esta a tres unidades do chao (colocar numa funcao)
        {
            rigidbody.drag = 1.5f;// Para calcular a resitencia do ar no objecto que tem o rigidBody (decellarate)
                                  //Turn into a function that computes add force based on input, gets a vector an adds force
            //fowardForce = transform.forward * acceleration * Input.GetAxis("Vertical");
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
        //if (Input.GetAxis("Vertical") >= 0)
        //    //turnForce = turnForce * Input.GetAxis("Horizontal");
        //else
        //{
        //    //turnForce = turnForce * -Input.GetAxis("Horizontal");
        //}
        //turnForce = turnForce * Time.fixedDeltaTime * rigidbody.mass;
        //rigidbody.AddTorque(turnForce, ForceMode.Force);

        if (Input.GetKey(KeyCode.Space) && GetComponent<ThrusterController>().strenght < GetComponent<ThrusterController>().maxStreghtStart)
        {
            GetComponent<ThrusterController>().distancePercent = 1;
            GetComponent<ThrusterController>().strenght += 10f;
            GetComponent<ThrusterController>().distanceMax = 2f;

        }
        else if (GetComponent<ThrusterController>().strenght > GetComponent<ThrusterController>().maxStrenght)
        {
            GetComponent<ThrusterController>().strenght -= 10f;

        }

        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -rotationAngle, ref rotationVelocity, smothTime, turnSpeed, Time.fixedDeltaTime);

        transform.eulerAngles = newRotation;
        if (Physics.Raycast(transform.position, -transform.up, 1f)&&transform.rotation.z<45)
        {
            lastPosition = transform.position;
            lastFoward = transform.forward;
            //Debug.DrawRay(transform.position, -transform.up, Color.black);
        }
    }
}



