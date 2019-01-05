using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectLine : MonoBehaviour
{

    public Material[] materials;
    public GameObject player;
    // Use this for initialization
    void Start()
    {
        GetComponent<MeshRenderer>().material = materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (player.GetComponent<ThrusterController>().inPlace)
            GetComponent<MeshRenderer>().material = materials[0];
        else
            GetComponent<MeshRenderer>().material = materials[1];

    }
}
