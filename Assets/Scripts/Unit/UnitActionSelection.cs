using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSelection : MonoBehaviour
{
    public static UnitActionSelection Instance { get; private set; }

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    public event EventHandler onSelectedUnitChanged;

    private bool isBusy;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError($"There are more than one instance for {this}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (isBusy) return;

        if(Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if(selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, unitLayerMask))
        {
            if(raycastHit.collider.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        onSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public void SetBusy()
    {
        isBusy = true;
    }

    public void ClearBusy()
    {
        isBusy = false;
    }
}
