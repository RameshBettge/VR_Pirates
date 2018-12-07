using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class DetachableBone : MonoBehaviour
{
    Rigidbody rb;
    Collider[] cols;

    float forceModifier = 100f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }

        if(cols.Length < 1)
        {
            Debug.LogWarning(name + " (DetachableBone) has no collider!");
        }
    }

    void Update()
    {
    }

    public void Detach(Vector3 force)
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(force * forceModifier);

        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }
    }
}
