﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: public Shoot()

public class Pistol : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform bulletSpawn;

    [SerializeField]
    Transform Trigger;

    [SerializeField]
    float activatedTriggerRotation = -20f;

    int magazineSize = 50;

    int bulletsInMagazine;

    private void Awake()
    {
        bulletsInMagazine = magazineSize;
    }

    public void SetTriggerRotation(float input)
    {
        Vector3 localEuler = Trigger.localEulerAngles;
        localEuler.x = activatedTriggerRotation * input;

        Trigger.localEulerAngles = localEuler;
    }

    public void Shoot()
    {
        if(bulletsInMagazine < 1)
        {
            //Debug.Log("Magazine empty.");
            return;
        }


        Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

        bulletsInMagazine--;
        //Debug.Log("pew! bullets left: " + bulletsInMagazine);
    }
}

public struct ShotInfo
{
    public Vector3 hitPos;
    public Vector3 shotForward;
    public float force;
    public int damage;
    float sqrMaxDistance;


    public ShotInfo(Vector3 hitPos, Vector3 shotForward, float force, int damage, float knockbackInfluenceDistance)
    {
        this.hitPos = hitPos;
        this.shotForward = shotForward.normalized;
        this.force = force;
        this.damage = damage;
        sqrMaxDistance = knockbackInfluenceDistance;
    }

    public float GetDistancePercentage(Transform t)
    {
        float sqrDistance = (t.position - hitPos).sqrMagnitude;
        sqrDistance = Mathf.Clamp(sqrDistance, 0.001f, sqrMaxDistance);

        // if distance is high -> percentage is low.
        float distancePercentage = 1 - (sqrDistance / sqrMaxDistance);

        return distancePercentage;
    }
}
