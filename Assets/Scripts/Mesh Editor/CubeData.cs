using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeData
{

    //public static CubeData SingletonCubeData=null;

    //public CubeData()
    //{
    //    if(SingletonCubeData==null)
    //    SingletonCubeData = this;
    //}

    public static Vector3[] vertices = {new Vector3(1,1,1),
                          new Vector3(-1,1,1),
                          new Vector3(-1,-1,1),
                          new Vector3(1,-1,1),
                          new Vector3(-1,1,-1),
                          new Vector3(1,1,-1),
                          new Vector3(1,-1,-1),
                          new Vector3(-1,-1,-1)
    };

    public static int[][] faceTri =
    {
        new int[]{0,1,2,3},
        new int[]{5,0,3,6},
        new int[]{4,5,6,7},
        new int[]{1,4,7,2},
        new int[]{5,4,1,0},
        new int[]{3,2,7,6},
    };

    public static Vector3[] faceVert(int dir, float scale, Vector3 cubePos)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            fv[i] = vertices[faceTri[dir][i]] * scale + cubePos;// multiply for the unit vector
        }
        return fv;
    }
    public static Vector3[] faceVert(Direction dir, float scale, Vector3 cubePos)
    {
        return faceVert((int)dir, scale, cubePos);//Calls the metod on top so we can have both methods (top one to deal with arrays)
    }
}
