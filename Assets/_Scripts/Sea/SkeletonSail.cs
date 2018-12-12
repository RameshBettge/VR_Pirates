using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSail : MonoBehaviour, IDamageable {
    [HideInInspector]
    public bool destroyed = false;

    int health;

    void IDamageable.TakeDamage(ShotInfo info)
    {
        Debug.Log(info.damage);

        health -= info.damage;

        if(health < 1)
        {
            destroyed = true;
            GetComponent<Collider>().enabled = false;
            // TODO: Change mesh
            GetComponent<Renderer>().enabled = false;
        }
    }
}
