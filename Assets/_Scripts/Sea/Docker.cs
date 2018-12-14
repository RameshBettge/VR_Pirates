using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    [SerializeField]
    EditorPath[] paths;

    //[SerializeField]
    //int startPathNode;

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

        EditorPath path = paths[UnityEngine.Random.Range(0, paths.Length)];

        Vector3 spawnPosition = path.pathObjs[0].position;
        //spawnPosition.x += UnityEngine.Random.Range(-2, 2);
        skeleton.transform.position = spawnPosition;
        //skeleton.transform.localRotation = spawnPoint.localRotation;
        skeleton.transform.rotation = Quaternion.identity;

        //skeleton.GetComponent<MoveOnPath>().currentWayPointID = startPathNode + 1;
        MoveOnPath moveScript = skeleton.GetComponent<MoveOnPath>();
        moveScript.currentWayPointID = 1;
        moveScript.pathToFollow = path;

        skeleton.transform.parent = ship;
    }
}
