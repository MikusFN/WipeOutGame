using UnityEngine;
using System.Collections;

public class LookAtController : MonoBehaviour
{
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool isLocked;
    public Transform target;
    private Vector3 lastPosition;

    public int backSpace = -1;
    public float height = 1;

    void Start()
    {
        //Screen.lockCursor = true;
        isLocked = false;
    }

    void Update()
    {
        if (!isLocked)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
                rotationY = Mathf.Clamp(rotationY, -90, 90);
            }
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

        if (Input.GetKeyDown(KeyCode.End))
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Confined) ? CursorLockMode.Confined : CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.L))
            isLocked = !isLocked;

        if (isLocked && target)
        {
            transform.position = target.position + target.up*height+target.forward*backSpace;//Vector3.Lerp(target.position - transform.forward * backSpace, transform.position, 0.9f);
            transform.LookAt(target);
        }
        //lastPosition = target.position;
    }
}







//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LookAtController : MonoBehaviour
//{

//   // public Transform mesh;
//    public float velocityScalar = 2.0f;
//    float lastPositionY;
//    string axis;
//    float mouseY = 0, mouseX=0;
//    float newPositon;
//    Transform lastPositon, actualPositon;
//    Quaternion lastRotation, newRotation;

//    private void Awake()
//    {
//        actualPositon = transform;
//        lastPositon = transform;
//        lastRotation = transform.rotation;
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {

//        //if (mesh)
//        //{
//            if (Input.GetAxis("Horizontal") != 0)
//                transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * velocityScalar;
//            if (Input.GetAxis("Vertical") != 0)
//                transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * velocityScalar;

//            //if (Input.mousePosition.x>0)
//            //{
//            //    mouseX = (Input.mousePosition.x - Screen.width * 0.5f);
//            //}
//            //else
//            //{
//            //    mouseX = (Input.mousePosition.x + Screen.width * 0.5f);

//            //}
//            //actualPositon.forward = Vector3.RotateTowards(actualPositon.forward, actualPositon.forward + new Vector3( mouseX* Time.deltaTime, (Input.mousePosition.y - Screen.height * 0.5f) * Time.deltaTime, 0), Mathf.Deg2Rad * 180.0f, 10);

//            transform.LookAt(transform.position+transform.forward* 5.0f);
//        //}

//        actualPositon = transform;

//    }
//}
