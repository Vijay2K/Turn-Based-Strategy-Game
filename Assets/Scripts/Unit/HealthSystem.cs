using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler onDead;
    public event EventHandler onGetDamage;

    [SerializeField] private int health = 100;

    private int maxHealth;

    private void Start() 
    {
        maxHealth = health;
    }

    public void Damage(int damageAmount)
    {
        health = Mathf.Max(health - damageAmount, 0);

        if(health <= 0)
        {
            Die();
        }

        Debug.Log("Health : " + health);
        onGetDamage?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        onDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormzlized()
    {
        return (float)health / maxHealth;
    }
}
