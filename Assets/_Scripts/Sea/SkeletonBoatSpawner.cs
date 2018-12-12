﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoatSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject boatPrefab;

    [SerializeField]
    SeaMovement sea;

    [SerializeField]
    Transform dockerParent;

    [SerializeField]
    float spawnInterval = 10f;

    [SerializeField]
    float spawnDistance = 350f;

    [SerializeField]
    float maxSpawnZOffset = 30f;

    [SerializeField]
    float attackConeNarrowness = 30f;

    float nextSpawn;

    Docker[] dockers;

    private void Start()
    {
        dockers = new Docker[dockerParent.childCount];

        for (int i = 0; i < dockers.Length; i++)
        {
            dockers[i] = dockerParent.GetChild(i).GetComponent<Docker>();
        }
    }

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            float zOffset = Random.Range(-maxSpawnZOffset, maxSpawnZOffset);


            attackConeNarrowness *= -1;

            Vector3 spawnPos = new Vector3(attackConeNarrowness, 0f, zOffset);
            spawnPos = spawnPos.normalized * spawnDistance;
            SpawnBoat(spawnPos);
            nextSpawn = Time.time + spawnInterval;
        }
    }

    void SpawnBoat(Vector3 pos)
    {
        GameObject boat = Instantiate(boatPrefab, pos, Quaternion.identity);

        Docker closestDocker = GetClosestDocker(pos);
        boat.GetComponent<BoatNavigation>().OnSpawn(closestDocker, sea);
    }

    Docker GetClosestDocker(Vector3 pos)
    {
        float least = Mathf.Infinity;
        Docker docker = dockers[0];

        for (int i = 0; i < dockers.Length; i++)
        {
            float sqrDist = (dockers[i].transform.position - pos).sqrMagnitude;
            if (sqrDist < least)
            {
                least = sqrDist;
                docker = dockers[i];
            }
        }
        return docker;
    }
}