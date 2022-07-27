using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler onTurnChanged;
    
    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError($"More than one instance for {this}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        onTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber() => turnNumber;
    public bool IsPlayerTurn() => isPlayerTurn;
}
