using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    private void Start()
    {
        gridSystemVisual.HideAllGridPosition();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            //unit.GetComponent<MoveAction>().GetValidGridPositions();
            gridSystemVisual.HideAllGridPosition();
            gridSystemVisual.ShowGridPositionList(unit.GetComponent<MoveAction>().GetValidGridPositions());
        }
    }
}
