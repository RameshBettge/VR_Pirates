using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class DetachableBone : MonoBehaviour
{
    public Limb limb;

    Rigidbody rb;
    Collider[] cols;

    float forceModifier = 100f;

    float maxDistance = 1f;

    bool detached;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }

        if(cols.Length < 1)
        {
            Debug.LogWarning(name + " (DetachableBone) has no collider!");
        }
    }

    public void TakeDamage(ShotInfo info)
    {
        limb.TakeDamage(info);
    }

    public void Detach(ShotInfo info)
    {
        if (detached) { return; }

        transform.parent = null;
        rb.isKinematic = false;

        Vector3 dist = transform.position - info.hitPos;



        //rb.AddForce(force * forceModifier);

        detached = true;
    }
}
