using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTotalHealth : MonoBehaviour
{
    [HideInInspector]
    public Vector3 bodyScale;

    [HideInInspector]
    public bool hasLostLimb = false;

    int health = 5;

    private void Awake()
    {
        bodyScale = transform.localScale;

        LimbHealth[] limbs =  GetComponentsInChildren<LimbHealth>();
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].totalHealth = this;
        }
    }
   

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
        // TODO: Call LoseLimb() on all limbs. Also instantiate Head with accessoires, torso and hips.

        throw new NotImplementedException();
    }
}
