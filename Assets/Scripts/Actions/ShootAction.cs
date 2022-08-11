using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State 
    {
        AIMING, SHOOTING, COOLOFF
    }
    
    [SerializeField] private int maxShootDistance = 7;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    public event EventHandler<OnShootEventArgs> onShoot;

    private State state;
    private float stateTimer;
    private bool canShoot;
    private Unit targetUnit;

    private void Update() 
    {
        if(!isActive)
            return;

        stateTimer -= Time.deltaTime;

        switch(state)
        {
            case State.AIMING:
                AimTowardsTheTarget();
                break;
            case State.SHOOTING:
                if(canShoot)
                {
                    Shoot();
                    canShoot = false;
                }
                break;
            case State.COOLOFF:
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
        Vector3 tragetDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(tragetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        onShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = this.targetUnit,
            shootingUnit = this.unit
        });

        targetUnit.Damage(40);
    }

    private void NextState()
    {
        switch(state)
        {
            case State.AIMING:
                state = State.SHOOTING;
                float shootingStateTimer = 0.1f;
                stateTimer = shootingStateTimer;
                break;
            case State.SHOOTING:
                state = State.COOLOFF;
                float coolOffStateTimer = 0.5f;
                stateTimer = coolOffStateTimer;
                break;
            case State.COOLOFF:
                ActionCompleted();
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {     
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.AIMING;
        float aimingStateTimer = 1f;
        stateTimer = aimingStateTimer;

        canShoot = true;
        
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++) 
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++) 
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;

                int gridDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(gridDistance > maxShootDistance) continue;

                if(!LevelGrid.Instance.HasAnyUnitAtGridPosition(gridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(gridPosition);
            }
        }

        return validGridPositionList;
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100
        };
    }

    public override string GetActionName()
    {
        return "shoot";
    }

    public override int GetActionCost()
    {
        return 1;
    }

    public Unit GetTargetUnit() => targetUnit;
    public int GetMaxShootingRange() => maxShootDistance;
}
