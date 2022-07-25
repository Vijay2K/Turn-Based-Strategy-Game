using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button nextTurnButtonUI;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start()
    {
        nextTurnButtonUI.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnNumber();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        UpdateTurnNumber();
    }

    private void UpdateTurnNumber()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }
}
