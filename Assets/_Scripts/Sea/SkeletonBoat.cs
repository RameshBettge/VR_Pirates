﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoat : MonoBehaviour, IDamageable
{
    [SerializeField]
    Transform skeletonParent;

    public Skeleton[] skeletons;

    [SerializeField]
    float boardingInterval = 0.5f;

    float nextBoarding;
    int index;

    private void Start()
    {
        skeletons = new Skeleton[skeletonParent.childCount];

        for (int i = 0; i < skeletons.Length; i++)
        {
            skeletons[i] = skeletonParent.GetChild(i).GetComponent<Skeleton>();

            // TODO: acces rigidbody via celina's script (skeleton needs to get a reference to that script in awake.)
            skeletons[i].GetComponent<Rigidbody>().isKinematic = true;
            skeletons[i].boarded = false;
        }

    }



    public bool CheckCrewDead()
    {
        bool allDead = true;

        for (int i = 0; i < skeletons.Length; i++)
        {
            if (!skeletons[i].destroyed)
            {
                allDead = false;
                break;
            }
        }
        if (allDead)
        {
            Debug.Log("Sinking");
        }

        return allDead;
    }

    public bool Board(Docker docker)
    {
        if (Time.timeSinceLevelLoad >= nextBoarding)
        {
            for (int i = 0; i < skeletons.Length; i++)
            {
                if (!skeletons[i].destroyed && !skeletons[i].boarded)
                {
                    docker.OnDocking(skeletons[i]);
                    skeletons[i].OnBoarding();
                    nextBoarding = Time.timeSinceLevelLoad + boardingInterval;

                    break;
                }
                else if (i == skeletons.Length - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void TakeDamage(ShotInfo info)
    {
        // if hit by cannonball
        if(info.damage > 50)
        {
            Debug.Log("Hit by cannonball");

            for (int i = 0; i < skeletons.Length; i++)
            {
                skeletons[i].TakeDamage(info);
            }
        }
    }
}
