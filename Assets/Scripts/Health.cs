using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum objectType
    {
        Player,
        Enemy
    }

    public objectType type;
    public float health;
    public float maxHealth = 100f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            EndOfLife();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    void EndOfLife()
    {
        switch (type)
        {
            case objectType.Player:
                break;
            case objectType.Enemy:
                break;
        }
    }
}
