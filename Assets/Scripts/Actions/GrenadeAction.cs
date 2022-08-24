using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private int maxThrowDistance = 7;
    [SerializeField] private Transform grenadePrefab;

    private void Update() 
    {
        if(!isActive)
            return;
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition gridPositionOffset = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + gridPositionOffset;

                if(!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;

                int gridDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(gridDistance > maxThrowDistance) continue;

                validGridPositionList.Add(gridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeTransform = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.SetUp(gridPosition, OnGrenadeBehaviourComplete);
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionCompleted();
    }
}
