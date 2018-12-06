using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTotalHealth : MonoBehaviour
{
    public Vector3 bodyScale;

    int health = 5;

    private void Awake()
    {
        bodyScale = transform.localScale;
    }

    public bool hasLostLimb = false;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
