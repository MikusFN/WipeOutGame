using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class SphereMove : MonoBehaviour
{
    Vector3 velocity;
    public float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        velocity = GetComponentInParent<BezierCurve>().GetVelocityCubic(0);
        this.transform.position = GetComponentInParent<BezierCurve>().GetPointCubic(0);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && t < 1)
            Debug.Log("move");
        if (Input.GetKeyDown(KeyCode.A) && t > 0)
            t -= 0.01f;

        velocity = GetComponentInParent<BezierCurve>().GetVelocityCubic(t);
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().velocity = velocity;
        this.transform.position = GetComponentInParent<BezierCurve>().GetPointCubic(t);
    }
}
