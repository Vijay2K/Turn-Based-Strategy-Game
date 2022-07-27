using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButtonUI;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisual;

    private void Start()
    {
        endTurnButtonUI.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisual();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisual();
    }

    private void UpdateTurnNumber()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisual()
    {
        endTurnButtonUI.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
