using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject bucketPrefab;
    public GameObject player;
    public Transform weaponHolder;

    public float bucketDistance = 0.75f;

    private bool holdingBucket = true;

    private GameObject bucket;

    Transform childBucket;
    public bool detachChild;

    float start;
    float end;
    public float force = 10f;

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
                start = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                end = Time.time;
                force = (end - start) * 1000;
                holdingBucket = false;
                bucket.GetComponent<Rigidbody>().useGravity = true;
                bucket.GetComponent<Rigidbody>().AddForce(-player.transform.right * force);
                //-player.transform.right -> look above comment
                if (detachChild == true)
                {
                    bucket.transform.parent = null;
                    Destroy(bucket, 3.0f);
                }
                bucket = Instantiate(bucketPrefab, bucketPrefab.transform.position, bucketPrefab.transform.rotation, weaponHolder);
                bucket.transform.SetSiblingIndex(0);
                holdingBucket = true;
            }
        }
    }
}