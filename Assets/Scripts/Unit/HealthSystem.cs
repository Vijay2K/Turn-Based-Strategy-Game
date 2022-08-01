using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public event EventHandler onDead;

    public void Damage(int damageAmount)
    {
        health = Mathf.Max(health - damageAmount, 0);
        if(health <= 0)
        {
            Die();
        }
        Debug.Log("Health : " + health);
    }

    private void Die()
    {
        onDead?.Invoke(this, EventArgs.Empty);
    }
}
