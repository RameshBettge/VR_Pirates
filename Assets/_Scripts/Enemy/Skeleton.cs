using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [HideInInspector]
    public bool hasLostLimb = false;

    DetachableBone[] bones;

    int health = 5;


    private void Awake()
    {
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
        // TODO: Also Detach all weapons.


        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].Detach(info);
        }
    }
}
