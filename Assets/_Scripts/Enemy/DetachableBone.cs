using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class DetachableBone : MonoBehaviour
{
    [HideInInspector]
    public Limb limb;

    [HideInInspector]
    public Skeleton skeleton;

    Rigidbody rb;
    Collider[] cols;

    int grabLayer;

    float forceModifier = 100f;

    float maxDistance = 1f;

    bool detached;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        grabLayer = LayerMask.NameToLayer("Grabbable");

        cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }

        if (cols.Length < 1)
        {
            Debug.LogWarning(name + " (DetachableBone) has no collider!");
        }
    }

    public void TakeDamage(ShotInfo info)
    {
        if (limb == null)
        {
            skeleton.TakeDamage(info, true);
            return;
        }
        limb.TakeDamage(info);
    }

    public void Detach(ShotInfo info)
    {
        if (detached)
        {
            KnockBack(info);
            return;
        }

        transform.parent = null;
        rb.isKinematic = false;

        Vector3 dist = transform.position - info.hitPos;



        SetGrabbable(transform);

        detached = true;

    }

    void KnockBack(ShotInfo info)
    {
        // TODO: Implement behaviour if seperated limb is shot again
    }

    void SetGrabbable(Transform t)
    {

        // TODO: Add GrabbableObject.cs to all bones
        t.gameObject.layer = grabLayer;

        for (int i = 0; i < t.childCount; i++)
        {
            SetGrabbable(t.GetChild(i));
        }
    }
}
