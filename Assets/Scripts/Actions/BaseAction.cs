using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler onActionStarted;
    public static event EventHandler onActionStopped;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidGridPositions();
        return validGridPositionList.Contains(gridPosition);
    }

    public virtual int GetActionCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        onActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionCompleted()
    {
        isActive = false;
        onActionComplete?.Invoke();
        onActionStopped?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit() => unit;
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public abstract List<GridPosition> GetValidGridPositions();
}
