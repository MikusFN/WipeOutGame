﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ItemsInSpline : MonoBehaviour
{


    public BezierSpline spline;
    public int frequenciaDeItem;
    public bool lookForward;
    public Transform[] items;
    private void Awake()
    {
        //Restrições ao items
        if (frequenciaDeItem <= 0 || items == null || items.Length == 0)
        {
            return;
        }
        //Percentagem para cada item
        float percentItem = 1f / (frequenciaDeItem * items.Length);
        
        for (int numeroItem = 0, f = 0; f < frequenciaDeItem; f++)
        {
            for (int i = 0; i < items.Length; i++, numeroItem++)
            {
                //Cada item é colocado na posição com o item do numero de item e é obtido na spline pela percentagem correspondente a cada item 
                Transform item = Instantiate(items[i]) as Transform;
                Vector3 position = spline.GetPointInSpline(numeroItem * percentItem);
                item.transform.localPosition = position;
                if (lookForward)
                {
                    //Colocar os objectos na direção da spline
                    item.transform.LookAt(position + spline.GetDirection(numeroItem * percentItem));
                }
                item.transform.parent = transform;
            }
        }

    }



    //if (spline.InLoop || stepSize == 1)
    //{
    //    stepSize = 1f / stepSize;
    //}
    //else
    //{
    //    stepSize = 1f / (stepSize - 1);
    //}

    //-----------------------//----------------------------//

    //Vector3 velocity;
    //public float t = 0;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    velocity = GetComponentInParent<BezierCurve>().GetVelocityCubic(0);
    //    this.transform.position = GetComponentInParent<BezierCurve>().GetPointCubic(0);
    //}

    //// Update is called once per frame
    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.D) && t < 1)
    //        Debug.Log("move");
    //    if (Input.GetKeyDown(KeyCode.A) && t > 0)
    //        t -= 0.01f;

    //    velocity = GetComponentInParent<BezierCurve>().GetVelocityCubic(t);
    //    if (GetComponent<Rigidbody>())
    //        GetComponent<Rigidbody>().velocity = velocity;
    //    this.transform.position = GetComponentInParent<BezierCurve>().GetPointCubic(t);
    //}
}
