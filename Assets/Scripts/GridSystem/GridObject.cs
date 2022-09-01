using System.Collections.Generic;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private IInteractables interactables;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public IInteractables GetInteractables()
    {
        return interactables;
    }

    public void SetInteractables(IInteractables interactables)
    {
        this.interactables = interactables;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())
        {
            return unitList[0];
        }

        return null;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public override string ToString()
    {
        string unitName = "";
        foreach(Unit unit in unitList)
        {
            unitName += "\n" + unit.ToString();
        }
        return gridPosition.ToString() + "\n" + unitName;
    }
}
