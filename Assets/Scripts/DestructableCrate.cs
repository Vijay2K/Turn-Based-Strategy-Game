using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler onCrateDestroy;

    private GridPosition gridPosition;

    private void Start() 
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        onCrateDestroy?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public GridPosition GetGridPosition() => gridPosition;
}
