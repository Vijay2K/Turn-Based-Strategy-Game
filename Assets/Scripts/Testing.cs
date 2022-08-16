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
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);

            List<GridPosition> pathList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition);
            for (int i = 0; i < pathList.Count - 1; i++)
            {
                Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(pathList[i]), LevelGrid.Instance.GetWorldPosition(pathList[i + 1]), Color.white, 10f);
            }
        }
    }    
}
