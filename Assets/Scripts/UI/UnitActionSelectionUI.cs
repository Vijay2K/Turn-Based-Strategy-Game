using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonUIPrefab;
    [SerializeField] private Transform actionButtonContainer;

    private void Start()
    {
        CreateActionUIButtons();
        UnitActionSelection.Instance.onSelectedUnitChanged += UnitActionSelection_OnSelectedUnitChanged;
    }

    private void CreateActionUIButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSelection.Instance.GetSelectedUnit();
        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonUIPrefab, actionButtonContainer);
            actionButtonTransform.GetComponent<ActionButtonUI>().SetBaseAction(baseAction);
        }
    }

    private void UnitActionSelection_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        CreateActionUIButtons();
    }
}
