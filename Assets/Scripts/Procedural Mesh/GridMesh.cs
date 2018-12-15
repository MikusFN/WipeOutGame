using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class GridMesh : MonoBehaviour
{
    //Tamanho da grid
    public int xSize, ySize;
    public BezierSpline spline;
    public GameObject walker;
    private Vector3[] grid;
    private Vector3[] gridInSpline;
    private Mesh mesh;
    private int heigh;
    private Vector3 positionMesh;


    float p;

    public void Start()
    {
        positionMesh = spline.GetPointInSpline(0);
        transform.position = positionMesh;
    }
    public void Update()
    {
        GenerateGrid();
        p += Time.deltaTime / 10;
        //Bound para o clamp de 0 a 1 da spline e os modos do objecto
        if (p > 1f)
        {
            p = 0;
        }

        
        positionMesh = spline.GetPointInSpline(p);
        //transform.position = positionMesh;
        //transform.LookAt(positionMesh + spline.GetDirection(p));

    }



    private void GenerateGrid()
{
    GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    GetComponent<MeshCollider>().sharedMesh = mesh;
    mesh.name = "Procedural Grid";

    //Vector que guarda a grid
    grid = new Vector3[(xSize + 1) * (ySize + 1)];
    Vector2[] uv = new Vector2[grid.Length];
    Vector4[] tangentes = new Vector4[grid.Length];
    float xT = transform.position.x;
    float yT = transform.position.y;
    float zT = transform.position.z;
    for (int i = 0, y = 0; y < ySize + 1; y++)
    {
        for (int x = 0; x < xSize + 1; x++, i++)
        {
            if (x % xSize == 0)
                heigh = 2;
            else
            {
                heigh = 0;
            }
            //Grelha com o avanço de um valor
            //grid[i] = new Vector3(x + xT, heigh + yT, y + zT);
            grid[i] = new Vector3(x, heigh, y);
            //Calculo de UV
            uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            //Para calculo dos bump maps
            tangentes[i] = new Vector4(1, 0, 0 - 1);
        }
    }
    // gridInSpline = new Vector3[grid.Length];
    //int l = 0;
    //foreach (Vector3 item in grid)
    //{
    //   gridInSpline[l]= item + spline.GetPointInSpline(l/gridInSpline.Length);
    //    l++;
    //}
    int[] triangleCoord = new int[xSize * ySize * 2 * 3];

    //Para ir na direçao dos ponteiros do relogio (Clock-Wise é a unica maneira da mesh ser visivel)
    for (int i = 0, j = 0, y = 0; y < ySize; j++, y++)//Row by row
    {
        for (int x = 0; x < xSize; i += 6, j++, x++)// coluna a colune
        {
            triangleCoord[0 + i] = j;
            //Partilham pontos
            triangleCoord[2 + i] = triangleCoord[3 + i] = 1 + j;
            triangleCoord[1 + i] = triangleCoord[4 + i] = xSize + 1 + j;
            triangleCoord[5 + i] = xSize + 2 + j;
        }
    }

    mesh.vertices = grid;
    mesh.triangles = triangleCoord;
    mesh.uv = uv;
    mesh.tangents = tangentes;
    mesh.RecalculateNormals();
}

    //public void OnDrawGizmos()
    //{

    //    if (grid == null)
    //        return;

    //    Gizmos.color = Color.green;

    //    for (int i = 0; i < grid.Length; i++)
    //    {
    //        Gizmos.DrawSphere(grid[i], 0.1f);
    //    }
    //}
}