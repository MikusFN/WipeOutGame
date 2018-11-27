using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{
    public float fallDelay;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.GetComponentInParent<Transform>().forward= Vector3.SlerpUnclamped(other.GetComponentInParent<Transform>().forward, transform.forward, 0.01f); 
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(FallDown());
            TilesManager.Instance.TilesMaker(); // fazer uma instancia de uma classe que seja estatica dentro da classe permite qu ela seja usada como um singleton.
        }
    }

    //CoRotine for a falldown of the tiles
    IEnumerator FallDown()
    {
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(2);
        switch (gameObject.name)
        {
            case "CaminhoFrente(Clone)":
                TilesManager.Instance.RecycledTiles[0].Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
            case "CaminhoDireita(Clone)":
                TilesManager.Instance.RecycledTiles[1].Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
            case "CaminhoTras(Clone)":
                TilesManager.Instance.RecycledTiles[2].Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
            case "CaminhoEsquerda(Clone)":
                TilesManager.Instance.RecycledTiles[3].Push(gameObject);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
