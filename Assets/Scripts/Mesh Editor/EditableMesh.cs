using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adiciona a dependency de ter que usar um mesh filter
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EditableMesh : MonoBehaviour
{

    Mesh mesh;

    private int[] triangles;
    private Vector3[] vertices;

    private List<int> trianglesList;
    private List<Vector3> verticesList;

    public int gridSizeX = 1, gridSizeY = 1;
    public float cellSize = 1, heightValue = 0.1f, cubeX = 1, cubeY = 1, cubeZ = 1;
    public Vector3 gridOffSet;
    float yValue = 0;

    //CubeData cubeData = new CubeData();

    public void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        MeshStartData();
        MeshMakeData();

        //GenerateVoxelMesh(new VoxelData());
        //MeshStartDataToArray();
    }

    private void GenerateVoxelMesh(VoxelData voxelData)
    {
        trianglesList = new List<int>();
        verticesList = new List<Vector3>();
        float vertexOffSet = cellSize * 0.5f;

        for (int i = 0; i < voxelData.Depth; i++)
        {
            for (int j = 0; j < voxelData.Width; j++)
            {
                if (voxelData.GetCell(j, i) == 0)
                    continue;
                else
                    MakeCubeVoxel((Direction)i, vertexOffSet, new Vector3(j * vertexOffSet, cubeY * vertexOffSet, i * vertexOffSet), j, i, voxelData);

            }
        }
    }

    private void MakeCubeVoxel(Direction dirVoxel, float vertexOffSet, Vector3 voxelPos, int x, int z, VoxelData voxelData)
    {
        trianglesList = new List<int>();
        verticesList = new List<Vector3>();
        float vertexOffSetVoxel = cellSize * 0.5f;

        for (int i = 0; i < 6; i++)
        {
            if (voxelData.GetNeightbor(x, z, dirVoxel) == 0)
                MakeFace((Direction)i, vertexOffSetVoxel, voxelPos);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    MeshMakeData();
    //    MeshStartData();

    //    //MeshStartDataToArray();
    //    //MeshUpdateData();
    //}


    private void MeshMakeData()
    {
        //MakeDicreteGrid();
        MakeContinuosGrid();
        //MakeCube();
    }

    private void MakeCube()
    {
        trianglesList = new List<int>();
        verticesList = new List<Vector3>();
        float vertexOffSet = cellSize * 0.5f;

        for (int i = 0; i < 6; i++)
        {
            MakeFace((Direction)i, vertexOffSet, new Vector3(cubeX * vertexOffSet, cubeY * vertexOffSet, cubeZ * vertexOffSet));
        }
    }

    private void MakeFace(Direction dir, float scaleCube, Vector3 cubePos)
    {

        verticesList.AddRange(CubeData.faceVert(dir, scaleCube, cubePos));
        int numbVert = verticesList.Count;

        trianglesList.Add(numbVert - 4 + 0);
        trianglesList.Add(numbVert - 4 + 1);
        trianglesList.Add(numbVert - 4 + 2);
        trianglesList.Add(numbVert - 4 + 0);
        trianglesList.Add(numbVert - 4 + 2);
        trianglesList.Add(numbVert - 4 + 3);


    }

    private void MakeDicreteGrid()
    {
        //Set array sizes de acordo com as dimensoes da grelha [linhas*colunas*numero de vertices por cada celula]
        vertices = new Vector3[gridSizeX * gridSizeY * 4];
        triangles = new int[gridSizeX * gridSizeY * 6];

        //set vertex offset so it can be in the middle of the cell
        float vertexOffSet = cellSize * 0.5f;

        //Interatores da grelha
        int l = 0, m = 0;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (j == i)
                {
                    yValue = Mathf.Exp(i * 0.1f);
                }
                else if (j > i)
                    yValue = Mathf.Exp(i * 0.1f);
                else
                    yValue = Mathf.Exp(j * 0.1f);

                //Posicionamento de cada celula na grelha
                Vector3 cellOffSet = new Vector3(i * cellSize, 0, j * cellSize);

                //Populate the vertices & triangles arrays
                vertices[l + 0] = new Vector3(-vertexOffSet, yValue, -vertexOffSet) + cellOffSet + gridOffSet;
                vertices[l + 1] = new Vector3(-vertexOffSet, yValue, vertexOffSet) + cellOffSet + gridOffSet;
                vertices[l + 2] = new Vector3(vertexOffSet, yValue, -vertexOffSet) + cellOffSet + gridOffSet;
                vertices[l + 3] = new Vector3(vertexOffSet, yValue, vertexOffSet) + cellOffSet + gridOffSet;

                //Set Triangles Order
                triangles[m + 0] = l;
                triangles[m + 1] = triangles[m + 4] = l + 1;
                triangles[m + 2] = triangles[m + 3] = l + 2;
                triangles[m + 5] = l + 3;

                //Loop para tringulos e vertices
                l += 4;
                m += 6;
            }
        }

    }

    private void MakeContinuosGrid()
    {
        //Set array sizes de acordo com as dimensoes da grelha [linha+1 * coluna+1] -> +1 é para fechar
        vertices = new Vector3[(gridSizeX + 1) * (gridSizeY + 1)];
        triangles = new int[gridSizeX * gridSizeY * 6];

        //set vertex offset so it can be in the middle of the cell
        float vertexOffSet = cellSize * 0.5f;

        //Interatores da grelha
        int l = 0, m = 0;

        for (int i = 0; i < gridSizeX + 1; i++)
        {
            for (int j = 0; j < gridSizeY + 1; j++)
            {
                //if (j == i)
                //{
                //    yValue = Mathf.Exp(i * heightValue);
                //}
                //else if (j > i)
                //    yValue = Mathf.Exp(i * 0.1f);
                //else
                //yValue = Mathf.Exp(j * 0.1f);

                //Populate the vertices & triangles arrays
                vertices[l] = new Vector3((i * cellSize) - vertexOffSet, yValue, (cellSize) - vertexOffSet);
                Instantiate<GameObject>(GameObject.CreatePrimitive(PrimitiveType.Plane), vertices[l], Quaternion.identity);
                l++;
            }
        }
        l = 0;


        for (int i = 0; i < gridSizeX; i++)
        {
            //for (int j = 0; j < gridSizeY; j++)
            //{
            //Set Triangles Order
            triangles[m + 0] = l;
            triangles[m + 1] = triangles[m + 4] = l + 1;
            triangles[m + 2] = triangles[m + 3] = l + (gridSizeX) + 1;
            triangles[m + 5] = l + (gridSizeY + 1) + 1;
            l++;
            m += 6;
            // }
            l++;
        }
        foreach (int item in triangles)
        {
            Debug.Log(item);
        }
    }

    private void MakeTriangle()
    {
        #region Triangles
        //Criaçao dos triangulos clockWise para a face ficar virada para cima (Como tem 4 vertices é um quad)
        vertices = new Vector3[] { new Vector3(0, VerticeController.verticeController.yValue, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1) };

        //Ordem dos triangulos
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        #endregion
    }

    private void MeshStartData()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private void MeshStartDataToArray()
    {
        mesh.Clear();
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
    }


    private void MeshUpdateData()
    {
        mesh.RecalculateNormals();
    }
}
