using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [HideInInspector]
    public bool hasLostLimb = false;

    int health = 5;

    private void Awake()
    {

        Limb[] limbs = GetComponentsInChildren<Limb>();
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].skeleton = this;
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Call LoseLimb() on all limbs. Also instantiate Head with accessoires, torso and hips.
        Debug.LogWarning("Skeleton.Die() is nto implemented yet!");
    }
}
