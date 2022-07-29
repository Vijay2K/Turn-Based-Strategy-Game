using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int maxShootDistance = 7;

    public override List<GridPosition> GetValidGridPositions()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for(int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for(int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;

                int gridDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (gridDistance > maxShootDistance) continue;

                if (!LevelGrid.Instance.HasAnyUnitAtGridPosition(gridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(gridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("Shooting");
    }


    public override string GetActionName()
    {
        return "shoot";
    }

    public override int GetActionCost()
    {
        return 1;
    }
}
