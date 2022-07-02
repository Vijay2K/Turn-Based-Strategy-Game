using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSelection.Instance.onSelectedUnitChanged += UnitActionSelection_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitActionSelection_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        meshRenderer.enabled = UnitActionSelection.Instance.GetSelectedUnit() == this.unit;
    }
}
