using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class RailMesh : MonoBehaviour {

    public BezierSpline spline;
    private Mesh mesh;
    private bool addCollider = true;

    // Use this for initialization
    public void Start()
    {

        //positionMesh = spline.GetPointInSpline(0);
        //transform.position = positionMesh;
        //GenerateGrid(spline.PointsMesh);
        

    }

    // Update is called once per frame
    public void Update()
    {
        if (addCollider)
        {
            if (tag == "RightRail")
            {
                GenerateMesh(spline.PointsRightRail, spline.InLoop);
            }
            else if (tag == "LeftRail")
            {
                GenerateMesh(spline.PointsLeftRail, spline.InLoop);
            }
            else if (tag == "CorrectLine")
            {
                GenerateMesh(spline.PointsMeshLine, spline.InLoop);

            }
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
        if (tag == "RightRail")
            mesh.name = "Procedural Right Rail";
        if (tag == "LeftRail")
            mesh.name = "Procedural Left Rail";
        if (tag == "CorrectLine")
            mesh.name = "Procedural Correct Line";
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

            if (i < splinePoints.Count -1 || isLooped)
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
}
