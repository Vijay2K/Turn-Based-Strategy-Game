using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitActionSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonUIPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointsTextUI;

    private List<ActionButtonUI> actionButtonList;

    private void Awake()
    {
        actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSelection.Instance.onSelectedUnitChanged += UnitActionSelection_OnSelectedUnitChanged;
        UnitActionSelection.Instance.onSelectedActionChanged += UnitActionSelection_OnSelectedActionChanged;
        UnitActionSelection.Instance.onActionStarted += UnitActionSelection_OnActionChanged;
        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
        Unit.onActionPointsChanged += Unit_OnActionPointChanged;

        CreateActionUIButtons();
        UpdateSelectedVisual();
        UpdateActionPoint();
    }

    private void CreateActionUIButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonList.Clear();

        Unit selectedUnit = UnitActionSelection.Instance.GetSelectedUnit();
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonUIPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonList.Add(actionButtonUI);
        }
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButton in actionButtonList)
        {
            actionButton.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoint()
    {
        Unit selectedUnit = UnitActionSelection.Instance.GetSelectedUnit();
        actionPointsTextUI.text = "Action points : " + selectedUnit.GetActionPoints();
    }

    private void UnitActionSelection_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        CreateActionUIButtons();
        UpdateSelectedVisual();
        UpdateActionPoint();
    }

    private void UnitActionSelection_OnSelectedActionChanged(object sender, EventArgs args)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSelection_OnActionChanged(object sender, EventArgs args)
    {
        UpdateActionPoint();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        UpdateActionPoint();
    }

    private void Unit_OnActionPointChanged(object sender, EventArgs args)
    {
        UpdateActionPoint();
    }

}
