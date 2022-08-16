using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }

    [SerializeField] private Transform gridDebugObjectPrefab;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private int width;
    private int height;
    private int cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError($"More than one instance for {this}");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem<PathNode>(10, 10, 2, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObject(gridDebugObjectPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPostion, GridPosition endGridPosition)
    {
        List<PathNode> openNodeList = new List<PathNode>();
        List<PathNode> closedNodeList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPostion);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openNodeList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPostion, endGridPosition));
        startNode.CalculateFCost();

        while(openNodeList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openNodeList);

            if(currentNode == endNode)
            {
                //Path find
                return CalculatePath(endNode);
            }

            openNodeList.Remove(currentNode);
            closedNodeList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourPathNodeList(currentNode))
            {
                if(closedNodeList.Contains(neighbourNode)) continue;
                int tentativeGCost = currentNode.GetGCost() + 
                            CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPreviousPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if(!openNodeList.Contains(neighbourNode))
                    {
                        openNodeList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int distance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);

        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);

        int remainingDistance = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remainingDistance;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourPathNodeList(PathNode currentPathNode)
    {
        List<PathNode> neighbourPathNodeList = new List<PathNode>();

        GridPosition currentGridPosition = currentPathNode.GetGridPosition();

        if(currentGridPosition.x + 1 < gridSystem.GetWidth())
        {
            neighbourPathNodeList.Add(GetNode(currentGridPosition.x + 1, currentGridPosition.z));
            
            if(currentGridPosition.z - 1 >= 0)
            {
                neighbourPathNodeList.Add(GetNode(currentGridPosition.x + 1, currentGridPosition.z - 1));
            }

            if(currentGridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourPathNodeList.Add(GetNode(currentGridPosition.x + 1, currentGridPosition.z + 1));
            }
        }

        if(currentGridPosition.x - 1 >= 0)
        {
            neighbourPathNodeList.Add(GetNode(currentGridPosition.x - 1, currentGridPosition.z));

            if(currentGridPosition.z - 1 >= 0)
            {
                neighbourPathNodeList.Add(GetNode(currentGridPosition.x - 1, currentGridPosition.z - 1));
            }                

            if(currentGridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourPathNodeList.Add(GetNode(currentGridPosition.x - 1, currentGridPosition.z + 1));
            }
        }

        if(currentGridPosition.z + 1 < gridSystem.GetHeight())
        {
            neighbourPathNodeList.Add(GetNode(currentGridPosition.x, currentGridPosition.z + 1));
        }

        if(currentGridPosition.z - 1 >= 0)
        {
            neighbourPathNodeList.Add(GetNode(currentGridPosition.x, currentGridPosition.z - 1));
        }

        return neighbourPathNodeList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();

        pathNodeList.Add(endNode);
        PathNode currentPathNode = endNode;

        while(currentPathNode.GetPreviousPathNode() != null)
        {
            pathNodeList.Add(currentPathNode.GetPreviousPathNode());
            currentPathNode = currentPathNode.GetPreviousPathNode();
        }

        pathNodeList.Reverse();
        List<GridPosition> gridPositionList = new List<GridPosition>();
        
        foreach(PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }
}
