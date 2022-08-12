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

    private Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive) return;

        float stoppingDistance = 0.1f;
        float distance = Vector3.Distance(transform.position, targetPosition);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (distance > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            onStopMoving?.Invoke(this, EventArgs.Empty);
            ActionCompleted();
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

                validGridPositionList.Add(gridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        onStartMoving?.Invoke(this, EventArgs.Empty);
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
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
