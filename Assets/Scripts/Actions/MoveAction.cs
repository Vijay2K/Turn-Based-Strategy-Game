using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int maxGridPosition = 1;

    public event EventHandler onStartMoving;
    public event EventHandler onStopMoving;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!isActive) return;

        float stoppingDistance = 0.1f;

        Vector3 targetPosition = positionList[currentPositionIndex];
        float distance = Vector3.Distance(transform.position, targetPosition);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (distance > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {
                onStopMoving?.Invoke(this, EventArgs.Empty);
                ActionCompleted();
            }
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for(int x = -maxGridPosition; x <= maxGridPosition; x++)
        {
            for(int z = -maxGridPosition; z <= maxGridPosition; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;
                if (unitGridPosition == gridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitAtGridPosition(gridPosition)) continue;

                if(!PathFinding.Instance.IsWalkableGridPosition(gridPosition)) continue;
                if(!PathFinding.Instance.HasAnyPath(unitGridPosition, gridPosition)) continue;

                int pathFindingDistanceMultiplier = 10;
                if(PathFinding.Instance.GetPathLength(unitGridPosition, gridPosition) > maxGridPosition *   pathFindingDistanceMultiplier) continue;

                validGridPositionList.Add(gridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(
                unit.GetGridPosition(), 
                gridPosition, 
                out int pathLength
            );

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        onStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10
        };
    }

    public override int GetActionCost()
    {
        return 1;
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
