using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform unitRagdollprefab;
    [SerializeField] private Transform originalRootbone;

    private HealthSystem healthSystem;

    private void Awake() 
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start() 
    {
        healthSystem.onDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs args)
    {
        Transform unitRagdollTransform = Instantiate(unitRagdollprefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = unitRagdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.SetUp(originalRootbone);
    }
}
