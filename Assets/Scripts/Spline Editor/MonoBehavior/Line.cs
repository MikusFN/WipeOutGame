using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Vector3 p0, p1, p2;
    [SerializeField]
    private float t = 0;
    [SerializeField]
    private GameObject prefab;

    private float lastT = 0;


    #endregion Fields

    #region Properties
    public Vector3 P0 { get { return p0; } set { p0 = value; } }
    public Vector3 P1 { get { return p1; } set { p1 = value; } }
    public Vector3 P2 { get { return p2; } set { p2 = value; } }

    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods
    void Update()
    {

        Debug.DrawLine(P0, P1);
        Debug.DrawLine(P1, P2);

        if (lastT != t && prefab)
        {
            Instantiate(/*GameObject.CreatePrimitive(PrimitiveType.Capsule)*/prefab, Vector3.Lerp(Vector3.Lerp(P0, P1, t), Vector3.Lerp(P1, P2, t), t), Quaternion.identity, this.transform);
        }
        lastT = t;
    }
    #endregion Methods

}
