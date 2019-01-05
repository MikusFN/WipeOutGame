using Assets.GeneralScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [SerializeField]
    [HideInInspector]
    private float steering, throttle;
    [SerializeField]
    [HideInInspector]
    private bool autoPilot;
    [SerializeField]
    [HideInInspector]
    private bool balanca;
    CarController car;
    LoadXMLData data;

    void Awake()
    {
        data = GetComponent<LoadXMLData>();
        car = GetComponent<CarController>();
        autoPilot = false;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LateStart());
    }
    
    IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        car.ActivateThrusters();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.P))
            autoPilot = !autoPilot;

        //if (data.SomaValores != 0)
        //{
        //    balanca = true;
        //}
        //else
        //{
        //    balanca = false;
        //}

        if (!autoPilot)
        {
            if (!balanca)
            {
                steering = Input.GetAxis("Horizontal");
                throttle = Input.GetAxis("Vertical");
            }
            else
            {
                steering = (data.Direita > data.Esquerda && (data.Direita - data.Esquerda)>0.1f ? data.Direita : -data.Esquerda) * 0.1f;
                throttle = (data.Frente > data.Tras&& (data.Frente - data.Tras) > 0.1f ? data.Frente : -data.Tras) * 0.1f;
            }
        }
        else
        {
            throttle = 1;
            steering = 0;
        }
        car.Move(steering, throttle, autoPilot);
    }
}
