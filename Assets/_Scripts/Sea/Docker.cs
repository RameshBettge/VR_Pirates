using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    public Transform spawnPoint;
    Transform ship;

    void Awake()
    {
        GetShip(transform.parent);
    }

    void GetShip(Transform t)
    {
        if(t.parent == null)
        {
            ship = t;
        }
        else
        {
            GetShip(t.parent);
        }
    }

    public void OnDocking(Skeleton skeleton)
    {
        // TODO: make celina's logic work from here
        skeleton.GetComponent<Rigidbody>().isKinematic = false;
        skeleton.transform.parent = ship;

        Vector3 spawnPosition = spawnPoint.position;
        spawnPosition.x += UnityEngine.Random.Range(-2, 2);
        skeleton.transform.position = spawnPosition;
    }
}
