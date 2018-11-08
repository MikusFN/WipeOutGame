using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    public float strenght, distance;
    public Transform[] thrusters;

    RaycastHit hit;
    Vector3 downForce;
    Rigidbody rigidbody;
    float distancePercent;

    void start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        foreach (Transform thruster in thrusters)
        {
            if(Physics.Raycast(thruster.position, -thruster.up, out hit, distance))
            {
                distancePercent = 1 - (hit.distance / distance);

                downForce = transform.up * strenght * distancePercent;

                if (rigidbody)
                {
                    downForce = downForce * Time.deltaTime * rigidbody.mass;

                    rigidbody.AddForceAtPosition(downForce, thruster.position);
                }
            }
        }    

    }
}
