using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonUIPrefab;
    [SerializeField] private Transform actionButtonContainer;

    private List<ActionButtonUI> actionButtonList;

    private void Awake()
    {
        actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSelection.Instance.onSelectedUnitChanged += UnitActionSelection_OnSelectedUnitChanged;
        UnitActionSelection.Instance.onSelectedActionChanged += UnitActionSelection_OnSelectedActionChanged;

        CreateActionUIButtons();
        UpdateSelectedVisual();
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

    private void UnitActionSelection_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        CreateActionUIButtons();
        UpdateSelectedVisual();
    }

    private void UnitActionSelection_OnSelectedActionChanged(object sender, EventArgs args)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButton in actionButtonList)
        {
            actionButton.UpdateSelectedVisual();
        }
    }
}
