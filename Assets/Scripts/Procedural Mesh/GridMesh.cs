using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class GridMesh : MonoBehaviour
{
    //Tamanho da grid
    //public int xSize, ySize;
    public BezierSpline spline;
    private Vector3[] grid;
    private Mesh mesh;
    private bool addCollider = true;

    public void Start()
    {

        //positionMesh = spline.GetPointInSpline(0);
        //transform.position = positionMesh;
        //GenerateGrid(spline.PointsMesh);
        
       


    }
    public void Update()
    {
        if (addCollider)
        {
            Vector3 initialPosition = spline.GetPointInSpline(1);
            initialPosition.x = -initialPosition.x;
            this.transform.position = initialPosition;
            GenerateMesh(spline.PointsMesh, spline.InLoop);

            GetComponent<MeshCollider>().sharedMesh = mesh;
            addCollider = false;
        }
        //p += Time.deltaTime / 10;
        ////Bound para o clamp de 0 a 1 da spline e os modos do objecto
        //if (p > 1f)
        //{
        //    p = 0;
        //}


        //positionMesh = spline.GetPointInSpline(p);
        //transform.position = positionMesh;
        //transform.LookAt(positionMesh + spline.GetDirection(p));

    }

    private void GenerateMesh(List<Vector3> splinePoints, bool isLooped)
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        mesh.name = "Procedural Grid";
        Vector3[] vertices = new Vector3[splinePoints.Count * 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangulos = new int[(2 * (splinePoints.Count - 1) + ((isLooped) ? 2 : 0)) * 3];
        int vertIndex = 0, triIndex = 0;
        for (int i = 0; i < splinePoints.Count; i++)
        {
            vertices[i] = splinePoints[i];

            float uvIndex = i / (float)(splinePoints.Count - 1);
            uvs[vertIndex] = new Vector2(0, uvIndex);
            uvs[vertIndex + 1] = new Vector2(1, uvIndex);

            if (i < splinePoints.Count - 1||isLooped)
            {
                triangulos[triIndex] = vertIndex;
                triangulos[triIndex + 1] = triangulos[triIndex + 4] = (vertIndex + 2) % vertices.Length;
                triangulos[triIndex + 2] = triangulos[triIndex + 3] = vertIndex + 1;

                //triangulos[triIndex + 3] = vertIndex + 1;
                //triangulos[triIndex + 4] = vertIndex + 2;
                triangulos[triIndex + 5] = (vertIndex + 3) % vertices.Length;
            }
            vertIndex += 2;
            triIndex += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangulos;
        mesh.uv = uvs;
    }

    //private void GenerateGrid(List<Vector3> splinePoints)
    //{
    //    GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    //    GetComponent<MeshCollider>().sharedMesh = mesh;
    //    mesh.name = "Procedural Grid";
    //    xSize = 1;
    //    ySize = splinePoints.Count;
    //    //Vector que guarda a grid
    //    grid = new Vector3[(xSize + 1) * (ySize + 1) + 1];
    //    Vector2[] uv = new Vector2[grid.Length];
    //    Vector4[] tangentes = new Vector4[grid.Length];

    //    for (int i = 0, y = 0; y < ySize; y++)
    //    {
    //        for (int x = 0; x < xSize; x++, i++)
    //        {
    //            //if (x % xSize == 0)
    //            //    heigh = 2;
    //            //else
    //            //{
    //            //    heigh = 0;
    //            //}
    //            //Grelha com o avanço de um valor
    //            //grid[i] = new Vector3(x + xT, heigh + yT, y + zT);
    //            //grid[i] = new Vector3(x, heigh, y);
    //            if (i != splinePoints.Count)
    //            {
    //                grid[i] = splinePoints[i];
    //                ////Calculo de UV
    //                uv[i] = new Vector2((float)x / ySize, (float)y / ySize);

    //                ////Para calculo dos bump maps
    //                tangentes[i] = new Vector4(1, 0, 0 - 1);
    //            }
    //            //else
    //            //{
    //            //    break;
    //            //}


    //        }
    //    }
    //    // gridInSpline = new Vector3[grid.Length];
    //    //int l = 0;
    //    //foreach (Vector3 item in grid)
    //    //{
    //    //   gridInSpline[l]= item + spline.GetPointInSpline(l/gridInSpline.Length);
    //    //    l++;
    //    //}
    //    int[] triangleCoord = new int[xSize * ySize * 2 * 3];

    //    //Para ir na direçao dos ponteiros do relogio (Clock-Wise é a unica maneira da mesh ser visivel)
    //    for (int i = 0, j = 0, y = 0; y < ySize; j++, y++)//Row by row
    //    {
    //        for (int x = 0; x < xSize; i += 6, j++, x++)// coluna a colune
    //        {
    //            triangleCoord[0 + i] = j;
    //            //Partilham pontos
    //            triangleCoord[2 + i] = triangleCoord[3 + i] = 1 + j;
    //            triangleCoord[1 + i] = triangleCoord[4 + i] = xSize + 1 + j;
    //            triangleCoord[5 + i] = xSize + 2 + j;
    //        }
    //    }

    //    mesh.vertices = grid;
    //    mesh.triangles = triangleCoord;
    //    mesh.uv = uv;
    //    mesh.tangents = tangentes;
    //    mesh.RecalculateNormals();
    //}

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