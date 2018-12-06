using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHealth : MonoBehaviour
{
    [SerializeField]
    GameObject limbPrefab;

    [HideInInspector]
    public EnemyTotalHealth totalHealth;

    [HideInInspector]
    public SkinnedMeshRenderer[] rends;

    [HideInInspector]
    public bool destroyed = false;

    List<Rigidbody> limbParts = new List<Rigidbody>();

    int health = 2;

    float debug = 0;

    private void Start()
    {
        rends = GetComponentsInChildren<SkinnedMeshRenderer>();
        //for (int i = 0; i < rends.Length; i++)
        //{
        //    rends[i].limb = this;
        //}

        //ShotInfo testInfo = new ShotInfo(Vector3.zero, Vector3.right, 1f, 1);
        //LoseLimb(testInfo);
    }

    private void Update()
    {
        // DEBUGGING

        if (Input.GetButtonDown("Jump"))
        {
            destroyed = false;
            ShotInfo testInfo = new ShotInfo(Vector3.zero, Vector3.right, 1f, 1);
            LoseLimb(testInfo);
        }
    }

    public void TakeDamage(ShotInfo info)
    {
        totalHealth.TakeDamage(info.damage);

        health--;

        if (health < 1 && !totalHealth.hasLostLimb)
        {
            LoseLimb(info);
        }
    }

    public void LoseLimb(ShotInfo info)
    {
        if (destroyed) { return; }

        Vector3 scale = totalHealth.bodyScale;

        GameObject bonePrefab = Resources.Load("Seperated Bone") as GameObject;
        GameObject subBonePrefab = Resources.Load("Seperated Sub-Bone") as GameObject;

        limbParts.Clear();

        for (int i = 0; i < rends.Length; i++)
        {
            // TODO: disable rends (commented out for testing)
            //rends[i].enabled = false;
        }

        GameObject seperated = Instantiate(limbPrefab);

        Transform rigBone = transform.GetComponent<SkinnedMeshRenderer>().bones[0];
        //seperated.transform.position = rigBone.position;
        //seperated.transform.localRotation = rigBone.rotation;
        ////seperated.transform.localEulerAngles += Vector3.up * 180;

        SetSeperatedTransform(transform, seperated.transform);

        seperated.transform.localScale = totalHealth.bodyScale;

        // TODO: give the visible Weapon a rigidbody and detach it from hand. This has to be done before instantiating the bones!

        // TODO: instantiate the limb prefab, set the position and rotation of each limb and then unparent all bones which have a rigidbody.
        //       then add force to those bones, depending on distance to info.hitPos and info.force.

        destroyed = true;
    }

    void SetSeperatedTransform(Transform original, Transform seperated)
    {
        Transform rigBone = original.GetComponent<SkinnedMeshRenderer>().bones[0];

        //seperated.parent = original.parent;
        //seperated.localScale = Vector3.one;

        //seperated.position = original.localPosition;
        //seperated.rotation = original.localRotation;

        //seperated.localPosition = rigBone.position;
        //seperated.localRotation = rigBone.rotation;

        //seperated.localScale = Vector3.one;


        for (int i = 0; i < original.childCount; i++)
        {
            SetSeperatedTransform(original.GetChild(i), seperated.GetChild(i));
        }
    }
}
