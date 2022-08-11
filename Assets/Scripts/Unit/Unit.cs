using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;

    public static event EventHandler onActionPointsChanged;
    public static event EventHandler onAnyUnitSpawned;
    public static event EventHandler onAnyUnitDead;

    private const int MAX_ACTION_POINTS = 2;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;
    private BaseAction[] baseActionArray;
    private HealthSystem healthSystem;
    private int actionPoints = MAX_ACTION_POINTS;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
        shootAction = GetComponent<ShootAction>();

        healthSystem.onDead += HealthSystem_OnDead;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
        onAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {            
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionCost());
            return true;
        }

        return false;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionCost();
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        onActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        if((isEnemy && !TurnSystem.Instance.IsPlayerTurn()) || 
            (!isEnemy && TurnSystem.Instance.IsPlayerTurn())) 
        {
            actionPoints = MAX_ACTION_POINTS;
            onActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs args)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        onAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetWorldPosition() 
    {
        return transform.position;
    }

    public MoveAction GetMoveAction() => moveAction;
    public SpinAction GetSpinAction() => spinAction;
    public ShootAction GetShootAction() => shootAction;
    public GridPosition GetGridPosition() => gridPosition;
    public BaseAction[] GetBaseActionArray() => baseActionArray;
    public int GetActionPoints() => actionPoints;
    public bool IsEnemy() => isEnemy;
}

