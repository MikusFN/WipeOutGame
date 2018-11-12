using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    public float strenght, distanceMax;
    public Transform[] thrusters;

    public Rigidbody rigidbody;


    void FixedUpdate()
    {
        RaycastHit hit;

        foreach (Transform thruster in thrusters)
        {
            Vector3 downForce;
            float distancePercent;

            if (Physics.Raycast(thruster.position, -thruster.up, out hit, distanceMax))
            {
                distancePercent = 1 - (hit.distance / distanceMax);
                
                    downForce = transform.up * strenght * distancePercent;
                               

                if (rigidbody)
                {
                    downForce = downForce * Time.fixedDeltaTime * rigidbody.mass;

                    rigidbody.AddForceAtPosition(downForce, thruster.position);
                }
            }
        }

    }
}