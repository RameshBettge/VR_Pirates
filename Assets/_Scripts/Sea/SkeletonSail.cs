using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSail : MonoBehaviour, IDamageable {
    [HideInInspector]
    public bool destroyed = false;

    int health;

    void IDamageable.TakeDamage(ShotInfo info)
    {
        Debug.Log("Sail took damage: " + info.damage);

        health -= info.damage;

        if(health < 1)
        {
            destroyed = true;
            GetComponent<Collider>().enabled = false;
            // TODO: Change mesh
            Renderer rend = GetComponent<Renderer>();
            if (rend == null)
            {
                rend = transform.parent.GetComponent<Renderer>();
            }
            rend.enabled = false;
        }
    }
}
