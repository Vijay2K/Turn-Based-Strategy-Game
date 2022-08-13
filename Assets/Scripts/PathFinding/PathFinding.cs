using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;

    private int width;
    private int height;
    private int cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        gridSystem = new GridSystem<PathNode>(10, 10, 2, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObject(gridDebugObjectPrefab);
    }
}
