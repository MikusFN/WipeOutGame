using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData : MonoBehaviour
{

    int[,] data = new int[,] { { 0, 1, 1 }, { 1, 1, 1 }, { 1, 1, 0 } };

    public int Width
    { get { return data.GetLength(0); } }

    public int Depth
    { get { return data.GetLength(1); } }

    public int GetCell(int x, int z)
    {
        return data[x, z];
    }

    public int GetNeightbor(int x, int z, Direction dir, int y = 0)
    {
        DataCoordinate offSetCheck = offSet[(int)dir];
        DataCoordinate neighBor = new DataCoordinate(x + offSetCheck.x, y + offSetCheck.y, z + offSetCheck.z);
        if (neighBor.x < 0 || neighBor.x >= Width || neighBor.y != 0 || neighBor.z >= Depth || neighBor.z < 0)
            return 0;
        else
            return GetCell(neighBor.x, neighBor.z);
    }

    public DataCoordinate[] offSet =
{
    new DataCoordinate(0,0,1),
    new DataCoordinate(1,0,0),
    new DataCoordinate(0,0,-1),
    new DataCoordinate(-1,0,0),
    new DataCoordinate(0,1,0),
    new DataCoordinate(0,-1,0),
    new DataCoordinate(0,0,1)
};
}

public struct DataCoordinate
{
    public int x, y, z;

    public DataCoordinate(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public enum Direction
{
    NORTH, EAST, SOUTH, WEST, UP, DOWN
}
