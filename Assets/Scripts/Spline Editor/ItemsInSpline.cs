using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ItemsInSpline : MonoBehaviour
{

    [HideInInspector]
    public float percentItem;
    public BezierSpline spline;
    public int frequenciaDeItem, width, height;
    public float widthLine;
    public bool lookForward;
    List<Vector3> pointsSpline = new List<Vector3>();


    public void Start()
    {
        //spline.GiveOnlyPoints();
        //Restrições ao items
        if (frequenciaDeItem <= 0)// || items == null || items.Length == 0)
        {
            return;
        }
        //Percentagem para cada item
        percentItem = 1f / (frequenciaDeItem); //* items.Length);

        for (int numeroItem = 0, f = 0; f < frequenciaDeItem; f++,numeroItem++)
        {
            
                //Cada item é colocado na posição com o item do numero de item e é obtido na spline pela percentagem correspondente a cada item 
                //Transform item = Instantiate(items[i]) as Transform;
                Vector3 position = spline.GetPointInSpline(numeroItem * percentItem);
                //item.transform.localPosition = position;
                //if (lookForward)
                //{
                //    //Colocar os objectos na direção da spline
                //    item.transform.LookAt(position + spline.GetDirection(numeroItem * percentItem));
                //}
                pointsSpline.Add(position);
                //Debug.Log(position);
                //item.transform.parent = transform;
            
        }
        spline.width = width;
        spline.lineWidth = widthLine;
        spline.height = height;
        spline.DefineMeshPoints(pointsSpline);

        //foreach (Vector3 point in spline.PointsMesh)
        //{
        //    if (prefab)
        //        Instantiate(prefab, point, Quaternion.identity);
        //}
        //CriacaoItemsSpline(ref items);
    }
    public void Update()
    {
        //for (int i = 0; i < spline.PointsMesh.Count; i++)
        //{
        //    if (i > 0)
        //        Debug.DrawLine(spline.GetterPointMesh(i), spline.GetterPointMesh(i - 1), Color.green);
        //}
    }


    public void InstantiationObject(int i, int numeroItem, float percentItem, ref Transform[] items)
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

    public void CriacaoItemsSpline(ref Transform[] items)
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
                InstantiationObject(i, numeroItem, percentItem, ref items);
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
