using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    [SerializeField] private int maxInteractDistance = 1;

    private void Update() 
    {
        if(!isActive)
            return;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPosition = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition gridPositionOffset = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + gridPositionOffset;

                if(!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;

                if(unitGridPosition == gridPosition) continue;

                IInteractables interactables = LevelGrid.Instance.GetInteractablesAtGridPosition(gridPosition);
                if(interactables == null) continue;

                validGridPosition.Add(gridPosition);
            }
        }

        return validGridPosition;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        IInteractables interactables = LevelGrid.Instance.GetInteractablesAtGridPosition(gridPosition);
        interactables.Interact(InteractActionComplete);
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    private void InteractActionComplete()
    {
        ActionCompleted();
    }

}
