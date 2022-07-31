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

                float rotationSpeed = 15f;
                Vector3 tragetDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(tragetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

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

    private void Shoot()
    {
        targetUnit.Damage();
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
                onActionComplete?.Invoke();
                isActive = false;
                break;
        }

        Debug.Log(state);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;

        Debug.Log("Aiming");
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.AIMING;
        float aimingStateTimer = 1f;
        stateTimer = aimingStateTimer;

        canShoot = true;
    }

    public override List<GridPosition> GetValidGridPositions()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
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

    public override string GetActionName()
    {
        return "shoot";
    }

    public override int GetActionCost()
    {
        return 1;
    }
}
