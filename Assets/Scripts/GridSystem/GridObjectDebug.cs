using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridObjectDebug : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugText;
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
        UpdateText();
    }

    private void UpdateText()
    {
        debugText.text = gridObject.ToString();
    }
}
