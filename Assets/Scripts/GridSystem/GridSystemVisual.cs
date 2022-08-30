using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridVisualType
{
    WHITE, RED, BLUE, YELLOW, RED_SOFT, GREEN, SOFT_GREEN
}

[Serializable]
public struct GridVisualTypeMaterial
{
    public GridVisualType gridVisualType;
    public Material gridMaterial;
}

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
              LevelGrid.Instance.GetWidth(),
                LevelGrid.Instance.GetHeight()
            ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridVisualTransform = Instantiate(gridVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = gridVisualTransform.GetComponent<GridSystemVisualSingle>();
            }
        }       
    }

    private void Start() 
    {        
        UnitActionSelection.Instance.onSelectedActionChanged += UnitActionSelection_OnSelectedActionChanged;
        LevelGrid.Instance.onAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        TurnSystem.Instance.onTurnChanged += TurnSystem_OnTurnChanged;
        Unit.onAnyUnitDead += Unit_OnAnyUnitDead;

        UpdateVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionInsideRangeList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition gridPositionOffset = gridPosition + new GridPosition(x, z);
                if(!LevelGrid.Instance.GetIsValidGridPosition(gridPositionOffset))
                    continue;

                int distance = Mathf.Abs(x) + Mathf.Abs(z);
                if(distance > range) 
                    continue;

                gridPositionInsideRangeList.Add(gridPositionOffset);
            }
        }

        ShowGridPositionList(gridPositionInsideRangeList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition unitGridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionRangeList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition gridPositionOffset =  new GridPosition(x, z);
                GridPosition gridPosition = unitGridPosition + gridPositionOffset;

                if(!LevelGrid.Instance.GetIsValidGridPosition(gridPosition)) continue;

                gridPositionRangeList.Add(gridPosition);
            }
        }

        ShowGridPositionList(gridPositionRangeList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualMaterial(gridVisualType));
        }
    }

    private void UpdateVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSelection.Instance.GetSelectedUnit();

        if(selectedUnit == null) return;

        BaseAction selectedAction = UnitActionSelection.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        switch(selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.WHITE;
                break;
            case ShootAction shooterAction:
                gridVisualType = GridVisualType.RED;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shooterAction.GetMaxShootingRange(), 
                GridVisualType.RED_SOFT);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.BLUE;
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.YELLOW;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.GREEN;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxAttackDistance(), 
                GridVisualType.SOFT_GREEN);
                break;
        }

        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private Material GetGridVisualMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial gridVisualMaterial in gridVisualTypeMaterialList)
        {
            if(gridVisualType != gridVisualMaterial.gridVisualType)
                continue;

            return gridVisualMaterial.gridMaterial;
        }

        Debug.LogError($"Couldn't find the material for {gridVisualType}");
        return null;
    }

    private void UnitActionSelection_OnSelectedActionChanged(object sender, EventArgs args)
    {
        UpdateVisual();
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs args)
    {
        UpdateVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs args)
    {
        UpdateVisual();
    }

    public void TurnSystem_OnTurnChanged(object sender, EventArgs args)
    {
        if(sender is TurnSystem)
        {
            TurnSystem turnSystem = sender as TurnSystem;
            if(!turnSystem.IsPlayerTurn())
            {
                HideAllGridPosition();
            }
        }
    }
}
