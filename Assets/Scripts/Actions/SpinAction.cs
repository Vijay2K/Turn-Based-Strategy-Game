using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive) return;

        float spinAmount = Time.deltaTime * 360f;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;

        if(totalSpinAmount >= 360)
        {
            ActionCompleted();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0;
        ActionStart(onActionComplete);
    }

    public override int GetActionCost()
    {
        return 2;
    }
    
    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridPositions()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>()
        {
            unitGridPosition
        };
    }
}
