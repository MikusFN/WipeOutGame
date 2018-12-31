using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    public float strenght, distanceMax, maxStreghtStart, maxStrenght;
    public Transform[] thrusters;
    public Transform[] colliders;

    public new Rigidbody rigidbody;

    Vector3 downForce;
    public float distancePercent;

    void FixedUpdate()
    {

        RaycastHit hit;

        for (int i =0;i< thrusters.Length;i++)
        {
            downForce = Vector3.zero;
            distancePercent = 0;

            if (Physics.Raycast(thrusters[i].position, -thrusters[i].up, out hit, distanceMax))
            {
                distancePercent = (1 - (hit.distance / distanceMax))*2;

                downForce = thrusters[i].up * strenght *distancePercent;
                colliders[i].position = hit.collider.ClosestPoint(thrusters[i].position);
                Debug.Log(hit.collider.tag);
                if (rigidbody)
                {
                    downForce = downForce * Time.fixedDeltaTime * rigidbody.mass;
                    Debug.DrawRay(thrusters[i].position, -downForce.normalized);
                    rigidbody.AddForceAtPosition(downForce, thrusters[i].position);
                }

            }
        }

    }
}