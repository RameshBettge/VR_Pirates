using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketRefiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Bucket bucket = other.GetComponent<Bucket>();
        if(bucket != null && bucket.filled == false)
        {
            bucket.Fill();
        }
    }
}
