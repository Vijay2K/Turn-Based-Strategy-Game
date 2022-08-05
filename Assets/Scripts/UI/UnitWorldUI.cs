using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Unit unit;
    [SerializeField] private HealthSystem healthSystem;

    private void Start() 
    {
        Unit.onActionPointsChanged += Unit_OnActionChanged;
        healthSystem.onGetDamage += HealthSystem_OnGetDamage;

        UpdateUnitActionPointsText();
        UpdateHealthBarUI();
    }

    private void Unit_OnActionChanged(object sender, EventArgs args)
    {
        UpdateUnitActionPointsText();
    }

    private void HealthSystem_OnGetDamage(object sender, EventArgs args)
    {
        UpdateHealthBarUI();
    }

    private void UpdateUnitActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBarUI()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormzlized();
    }
}
