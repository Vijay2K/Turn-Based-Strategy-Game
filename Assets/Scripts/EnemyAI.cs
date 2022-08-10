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
                    state = State.WaitForEnemyTurn;
                    TurnSystem.Instance.NextTurn();
                }
                break;
            case State.Busy:
                break;
        }

        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            TurnSystem.Instance.NextTurn();
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
}
