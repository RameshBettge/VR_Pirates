using UnityEngine;
using UnityEngine.UI;

public class Throwing : MonoBehaviour
{
    [SerializeField]
    Text stockDisplay;

    public GameObject bucketPrefab;
    public GameObject player;
    public Transform weaponHolder;
    public Transform ship;

    public float bucketDistance = 0.75f;
    float force;

    [SerializeField]
    float maxForce = 5000f;
    [SerializeField]
    float forceModifier = 2000f;

    public float height = 100f;

    public bool detachChild;

    private GameObject bucket;
    Transform childBucket;

    private bool holdingBucket = true;

    float start;
    float end;

    //[HideInInspector]
    [SerializeField]
    int stock = 3;

    private void OnEnable()
    {
        if (stock > 0)
        {
            if(bucket == null) { return; }
            Bucket bucketScript = bucket.GetComponent<Bucket>();

            if (!bucketScript.filled)
            {
                bucketScript.Fill();
                stock--;
            }
        }
    }

    void Start()
    {
        bucket = Instantiate(bucketPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f), weaponHolder);
        bucket.transform.SetSiblingIndex(0);
        bucket.GetComponent<Rigidbody>().isKinematic = true;
        bucket.GetComponent<Collider>().enabled = false;

        UpdateStock(0);

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
                force = (end - start) * forceModifier;
                if (force > maxForce)
                {
                    force = maxForce;
                }

                holdingBucket = false;
                bucket.GetComponent<Rigidbody>().isKinematic = false;
                bucket.GetComponent<Rigidbody>().useGravity = true;
                bucket.GetComponent<Collider>().enabled = true;

                bucket.GetComponent<Rigidbody>().AddForce(-player.transform.right * force + player.transform.up * height);
                //-player.transform.right -> look above comment
                if (detachChild == true)
                {
                    bucket.transform.parent = ship;
                    //Destroy(bucket, 2f);
                }
                Vector3 spawnPos = player.transform.position + player.transform.forward * bucketDistance + -player.transform.right * 0.6f;
                bucket = Instantiate(bucketPrefab, spawnPos, bucketPrefab.transform.rotation, weaponHolder);
                bucket.GetComponent<Rigidbody>().isKinematic = true;
                bucket.GetComponent<Collider>().enabled = false;


                bucket.transform.SetSiblingIndex(0);

                if (stock > 0)
                {
                    bucket.GetComponent<Bucket>().Fill();

                    UpdateStock(-1);
                }

                holdingBucket = true;
            }
        }
    }

    public void UpdateStock(int num)
    {
        stock += num;

        stockDisplay.text = "Buckets o' Water: " + stock.ToString();
    }
}