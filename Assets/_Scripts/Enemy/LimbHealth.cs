using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHealth : MonoBehaviour
{
    [SerializeField]
    GameObject limbPrefab;

    public EnemyTotalHealth totalHealth;

    public Bone[] subBones;

    int health = 2;

    private void Start()
    {
        subBones = GetComponentsInChildren<Bone>();
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

    void LoseLimb(ShotInfo info)
    {
        Vector3 scale = totalHealth.bodyScale;

        GameObject bonePrefab = Resources.Load("Seperated Bone") as GameObject;
        GameObject subBonePrefab = Resources.Load("Seperated Sub-Bone") as GameObject;

        for (int i = 0; i < subBones.Length; i++)
        {
            subBones[i].rend.enabled = false;

            // TODO: instantiate the limb prefab, set the position and rotation of each limb and then unparent all bones which have a rigidbody.
            //       then add force to those bones, depending on distance to info.hitPos and info.force.
            
        }
    }
}
