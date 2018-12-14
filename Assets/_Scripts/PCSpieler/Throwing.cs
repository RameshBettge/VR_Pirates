﻿using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject bucketPrefab;
    public GameObject player;
    public Transform weaponHolder;
    public Transform ship;

    public float bucketDistance = 0.75f;
    public float force = 10f;
    public float height = 100f;

    public bool detachChild;

    private GameObject bucket;
    Transform childBucket;

    private bool holdingBucket = true;

    float start;
    float end;

    //[HideInInspector]
    public int stock = 3;

    void Start()
    {
        bucket = Instantiate(bucketPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f), weaponHolder);
        bucket.transform.SetSiblingIndex(0);
    }

    void Update()
    {
        BucketThrow();
    }

    void BucketThrow()
    {
        if (holdingBucket)
        {
            bucket.transform.position = player.transform.position + player.transform.forward * bucketDistance + -player.transform.right * 0.6f;
            //-player.transform.right, because pivot of object is twisted
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                start = Time.timeSinceLevelLoad;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                end = Time.timeSinceLevelLoad;
                force = (end - start) * 2000;
                holdingBucket = false;
                bucket.GetComponent<Rigidbody>().useGravity = true;
                bucket.GetComponent<Rigidbody>().AddForce(-player.transform.right * force + player.transform.up * height);
                //-player.transform.right -> look above comment
                if (detachChild == true)
                {
                    bucket.transform.parent = ship;
                    //Destroy(bucket, 2f);
                }
                bucket = Instantiate(bucketPrefab, bucketPrefab.transform.position, bucketPrefab.transform.rotation, weaponHolder);
                bucket.transform.SetSiblingIndex(0);
                
                if(stock > 0)
                {
                    bucket.GetComponent<Bucket>().filled = true;
                    stock--;
                }

                holdingBucket = true;
            }
        }
    }
}