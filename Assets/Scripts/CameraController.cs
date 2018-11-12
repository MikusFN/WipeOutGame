using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float height, backStep, minimalHeight, lag = 0.18f, unitsOnfront=5.0f;

    private Vector3 positionVelocity;

    void FixedUpdate()
    {
        Vector3 newPosition = target.position + (target.forward * -backStep);
        newPosition.y = Mathf.Max(newPosition.y + height, minimalHeight);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref positionVelocity, lag);
        Vector3 targetPoint = target.position + (target.forward * unitsOnfront);
        transform.LookAt(targetPoint);
    }
}
