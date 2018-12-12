using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [HideInInspector]
    public ScriptableCurve boneDetachCurve;

    [HideInInspector]
    public bool hasLostLimb = false;

    [HideInInspector]
    public bool destroyed = false;

    [HideInInspector]
    public bool boarded = false;

    DetachableBone[] bones;

    EnemyBehaiviour behaviour;

    int health = 5;


    private void Awake()
    {
        boneDetachCurve = Resources.Load<ScriptableCurve>("ScriptableObjects/BoneDetachCurve");

        bones = GetComponentsInChildren<DetachableBone>();

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].skeleton = this;
        }

        Limb[] limbs = GetComponentsInChildren<Limb>();
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].skeleton = this;
        }

        behaviour = GetComponent<EnemyBehaiviour>();
        // TODO: Remove null check when prefab is done
        if (behaviour != null)
        {
            behaviour.enabled = false;
        }
    }


    public void TakeDamage(ShotInfo info, bool bodyShot = false)
    {
        health -= info.damage;

        if (health < 1)
        {
            Die(info);
        }
    }

    private void Die(ShotInfo info)
    {
        // TODO: Make sure all equipped weapons are detached! Stop Celina's logic
        if (destroyed) { return; }

        for (int i = 0; i < bones.Length; i++)
        {
            // TODO: Despawn bones
            bones[i].Detach(info);
        }

        destroyed = true;

        if (behaviour != null)
        {
            if (boarded)
            {
                behaviour.Die();
            }
            else
            {
                // TODO: Check if destroying is save and not despawning loose bones/items
                Destroy(this);
                Destroy(gameObject);
            }
        }
    }
}
