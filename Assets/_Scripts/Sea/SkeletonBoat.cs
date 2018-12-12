using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoat : MonoBehaviour
{
    [SerializeField]
    Skeleton[] skeletons;

    [SerializeField]
    float boardingInterval = 0.5f;

    float nextBoarding;
    int index;

    private void Start()
    {
        for (int i = 0; i < skeletons.Length; i++)
        {
            // TODO: acces rigidbody via celina's script (skeleton needs to get a reference to that script in awake.)
            skeletons[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public bool Board(Docker docker)
    {
        if (Time.time >= nextBoarding)
        {
            for (int i = 0; i < skeletons.Length; i++)
            {
                if (!skeletons[i].destroyed && !skeletons[i].boarded)
                {
                    docker.OnDocking(skeletons[i]);
                    skeletons[i].boarded = true;
                    nextBoarding = Time.time + boardingInterval;

                    break;
                }
                else if(i == skeletons.Length - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
