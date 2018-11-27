using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticeController : MonoBehaviour
{

    public float yValue;

    //Singleton
    public static VerticeController verticeController;

    private void Awake()
    {
        verticeController = this;
    }
}
