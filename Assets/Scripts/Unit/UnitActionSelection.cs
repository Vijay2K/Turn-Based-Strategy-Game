using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSelection : MonoBehaviour
{
    public static UnitActionSelection Instance { get; private set; }

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    public event EventHandler onSelectedUnitChanged;
    public event EventHandler onSelectedActionChanged;
    public event EventHandler<bool> onBusyChanged;
    public event EventHandler onActionStarted;

    private bool isBusy;
    private BaseAction selectedAction;

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

    private void Start()
    {
        SetSelectedAction(selectedUnit.GetAction<MoveAction>());
        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if(UnitManager.Instance.GetFriendlyUnitList().Count <= 0) return;

        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, unitLayerMask))
            {
                if (raycastHit.collider.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit) return false;
                    if (unit.IsEnemy()) return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void HandleSelectedAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) return;

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            onActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(selectedUnit.GetAction<MoveAction>());
        onSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        this.selectedAction = baseAction;
        onSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public void SetBusy()
    {
        isBusy = true;
        onBusyChanged?.Invoke(this, isBusy);
    }

    public void ClearBusy()
    {
        isBusy = false;
        onBusyChanged?.Invoke(this, isBusy);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            if(UnitManager.Instance.GetFriendlyUnitList().Count <= 0) return;
            if(selectedUnit != null) return;

            SetSelectedUnit(UnitManager.Instance.GetFriendlyUnitList()[0]);
        }        
    }
}
