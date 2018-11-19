using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    public float strenght, distanceMax;
    public Transform[] thrusters;

    public Rigidbody rigidbody;

    Vector3 downForce;
    public float distancePercent;

    void FixedUpdate()
    {

        RaycastHit hit;

        foreach (Transform thruster in thrusters)
        {
            downForce = Vector3.zero;
            distancePercent = 0;

            if (Physics.Raycast(thruster.position, -thruster.up, out hit, distanceMax))
            {
                distancePercent = 1 - (hit.distance / distanceMax);

                downForce = transform.up * strenght * distancePercent;

                if (rigidbody)
                {
                    downForce = downForce * Time.fixedDeltaTime * rigidbody.mass;

                    rigidbody.AddForceAtPosition(downForce, thruster.position);
                    Debug.DrawRay(thruster.position, downForce);
                }

            }
        }

    }
}