using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitForEnemyTurn, TakingTurn, Busy
    }

    private State state;
    private float timer;

    private void Awake() 
    {
        state = State.WaitForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
            return;

        switch(state)
        {
            case State.WaitForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if(timer <= 0f)
                {
                    if(TryTakeEnemyAIAction(SetStateTakeAction))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private void SetStateTakeAction()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyActionComplete)
    {
        Debug.Log("Taking Action");
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyActionComplete)
    {
        GridPosition actionGridPosition = enemyUnit.GetGridPosition();
        SpinAction spinAction = enemyUnit.GetSpinAction();

        Debug.Log(spinAction);

        if(!spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }
        
        if(!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            return false;
        }

        spinAction.TakeAction(actionGridPosition, onEnemyActionComplete);
        return true;
    }
}
