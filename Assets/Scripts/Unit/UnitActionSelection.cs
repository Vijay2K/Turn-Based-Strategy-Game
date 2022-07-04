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
        if(Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            selectedUnit.GetMoveAction().Move(MouseWorld.GetPosition());
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
}
