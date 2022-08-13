using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridObjectDebug : MonoBehaviour
{
    [SerializeField] private TextMeshPro debugText;
    
    private object gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
        debugText.text = gridObject.ToString();
    }
}
