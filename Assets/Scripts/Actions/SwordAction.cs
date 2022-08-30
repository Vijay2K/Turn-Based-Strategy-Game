using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler onAnySwordHit;
    public event EventHandler onSwordActionStart;
    public event EventHandler onSwordActionCompleted;

    [SerializeField] private int maxAttackDistance = 1;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    }

    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private void Update() 
    {
        if(!isActive) 
            return;

        stateTimer -= Time.deltaTime;

        switch(state)
        {
            case State.SwingingSwordBeforeHit:
                AimTowardsTheTarget();
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if(stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void AimTowardsTheTarget()
    {
        float rotationSpeed = 15f;
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void NextState()
    {
        switch(state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                float afterHitStateTimer = 0.5f;
                stateTimer = afterHitStateTimer;
                targetUnit.Damage(100);
                onAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                onSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionCompleted();
                break;
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
            {
                GridPosition gridPositionOffset = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + gridPositionOffset;

                if(!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;

                if(!LevelGrid.Instance.HasAnyUnitAtGridPosition(gridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                if(unitGridPosition == gridPosition) continue;

                validGridPositionList.Add(gridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTimer = 0.7f;
        stateTimer = beforeHitStateTimer;

        onSwordActionStart?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }
    
    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200 
        };
    }

    public int GetMaxAttackDistance() => maxAttackDistance;
}
