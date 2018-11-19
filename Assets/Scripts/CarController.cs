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
    public Rigidbody rigidbody;
    public GameObject track;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 fowardForce = Vector3.zero;
        if (Physics.Raycast(transform.position, -transform.up, 1f))//Se esta a tres unidades do chao (colocar numa funcao)
        {
            rigidbody.drag = 1f;// Para calcular a resitencia do ar no objecto que tem o rigidBody (decellarate)
                               //Turn into a function that computes add force based on input, gets a vector an adds force
            fowardForce = transform.forward * acceleration * Input.GetAxis("Vertical");
            fowardForce = fowardForce * Time.fixedDeltaTime * rigidbody.mass;
            rigidbody.AddForce(fowardForce);
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
                transform.forward = track.GetComponentInChildren<TilesManager>().currentTile.transform.forward;
                transform.position = track.GetComponentInChildren<TilesManager>().currentTile.transform.position + Vector3.up * 5;
            }
        }

        Vector3 turnForce = Vector3.up * rotationRate;
        if (Input.GetAxis("Vertical") >= 0)
            turnForce = turnForce * Input.GetAxis("Horizontal");
        else
        {
            turnForce = turnForce * -Input.GetAxis("Horizontal");
        }
        turnForce = turnForce * Time.fixedDeltaTime * rigidbody.mass;
        rigidbody.AddTorque(turnForce);

        if (Input.GetKey(KeyCode.Space) && GetComponent<ThrusterController>().strenght < 1000)
        {
            GetComponent<ThrusterController>().distancePercent = 1;
            GetComponent<ThrusterController>().strenght += 10f;
            GetComponent<ThrusterController>().distanceMax = 2f;

        }
        else if (GetComponent<ThrusterController>().strenght > 200)
        {
            GetComponent<ThrusterController>().strenght -= 10f;
            GetComponent<ThrusterController>().distanceMax = 1f;

        }

        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -rotationAngle, ref rotationVelocity, smothTime, turnSpeed, Time.fixedDeltaTime);

        transform.eulerAngles = newRotation;

        Debug.Log(rigidbody.angularVelocity);
    }
}



