using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform debugPrefab;
    private GridSystem gridSystem;

    private void Start()
    {
        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugObject(debugPrefab);
    }

    private void Update()
    {
        //Debug.Log($"Mouse Position : {MouseWorld.GetPosition()} and GridPosition : {gridSystem.GetGridPosition(MouseWorld.GetPosition())}");
    }
}
