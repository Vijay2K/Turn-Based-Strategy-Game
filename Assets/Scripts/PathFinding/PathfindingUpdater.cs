using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start() 
    {
        DestructableCrate.onCrateDestroy += DestructableCrate_OnCrateDestory;
    }

    private void DestructableCrate_OnCrateDestory(object sender, EventArgs args)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        PathFinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
