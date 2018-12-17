using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferFishSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject pufferFishPrefab;

    //Vector3 pufferOffset = new Vector3(1.7467f, -0.16686f, 3.1605f);

    Transform[] spawnPoints;

    float minForce = 34f;
    float maxForce = 44f;

    float spawnInterval = 8f;

    float nextSpawn;

    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        SpawnPuffer();

        nextSpawn = spawnInterval;
    }

    void Update()
    {
        if(Time.timeSinceLevelLoad>= nextSpawn)
        {
            SpawnPuffer();
            nextSpawn = Time.timeSinceLevelLoad+ spawnInterval;
        }
    }

    void SpawnPuffer()
    {
        int spawnNum = UnityEngine.Random.Range(0, spawnPoints.Length);

        GameObject puffer = Instantiate(pufferFishPrefab, spawnPoints[spawnNum].position /*+ pufferOffset*/, Quaternion.identity, transform.parent);
        Rigidbody rb = puffer.GetComponent<Rigidbody>();

        float force = UnityEngine.Random.Range(minForce, maxForce);
        rb.AddForce(spawnPoints[spawnNum].forward * force, ForceMode.VelocityChange);

        //puffer.transform.position = spawnPoints[spawnNum].position + pufferOffset;

        Vector3 randomRot = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            randomRot[i] = UnityEngine.Random.Range(-300f, 300f);
        }

        rb.AddTorque(randomRot);

    }

    void SetCorrectPos(Transform puffer, Vector3 pos)
    {

    }
}
