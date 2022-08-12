using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private int cellSize;
    private TGridObject[,] gridObjectArray;

    public GridSystem(int width, int height, int cellSize, 
            Func<GridSystem<TGridObject>, GridPosition, TGridObject> CreateGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridObjectArray = new TGridObject[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = CreateGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public void CreateDebugObject(Transform debugPrefab)
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugPrefabInstance = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridObjectDebug gridObjectDebug = debugPrefabInstance.GetComponent<GridObjectDebug>();
                gridObjectDebug.SetGridObject(GetGridObject(gridPosition) as GridObject);
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
               gridPosition.z >= 0 && 
               gridPosition.x < width && 
               gridPosition.z < height;
    }
}

public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position && x == position.x && z == position.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public static bool operator==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator!=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator+(GridPosition a, GridPosition b)
    {
        int x = a.x + b.x;
        int z = a.z + b.z;
        return new GridPosition(x, z);
    }

    public static GridPosition operator-(GridPosition a, GridPosition b)
    {
        int x = a.x - b.x;
        int z = a.z - b.z;

        return new GridPosition(x, z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override string ToString()
    {
        return $"({x}, {z})";
    }

}