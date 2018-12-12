using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{

    Transform spawnPoint;

    public void OnDocking(int numberOfSkeletons)
    {
        for (int i = 0; i < numberOfSkeletons; i++)
        {
            SpawnSkeleton();
        }
    }

    private void SpawnSkeleton()
    {
        // TODO: Spawn Skeleton at spawnPoint.

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = spawnPoint.position;
    }
}
