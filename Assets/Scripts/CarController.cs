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


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1f))//Se esta a tres unidades do chao (colocar numa funcao)
        {
            rigidbody.drag = 1;// Para calcular a resitencia do ar no objecto que tem o rigidBody (decellarate)
            //Turn into a function that computes add force based on input, gets a vector an adds force
            Vector3 fowardForce = transform.forward * acceleration * Input.GetAxis("Vertical");
            fowardForce = fowardForce * Time.fixedDeltaTime * rigidbody.mass;
            rigidbody.AddForce(fowardForce);
        }
        else
        {
            rigidbody.drag = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                transform.Rotate(transform.forward.normalized, 180);
            }
        }

        Vector3 turnForce = Vector3.up * rotationRate * Input.GetAxis("Horizontal");
        turnForce = turnForce * Time.fixedDeltaTime * rigidbody.mass;
        rigidbody.AddTorque(turnForce);

        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -rotationAngle, ref rotationVelocity, smothTime, turnSpeed, Time.fixedDeltaTime);

        transform.eulerAngles = newRotation;


    }
}



