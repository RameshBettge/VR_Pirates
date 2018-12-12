using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class DetachableBone : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public Limb limb;

    [HideInInspector]
    public Skeleton skeleton;

    Rigidbody rb;
    Collider[] cols;

    int grabLayer;

    float maxDistance = 1f;
    float sqrMaxDistance;

    bool detached;

    float detachForceMultiplier = 0.22f;
    float knockbackForceMultiplier = 5f;

    void Start()
    {
        sqrMaxDistance = maxDistance * maxDistance;

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
        if (detached)
        {
            KnockBack(info);
        }

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
            return;
        }

        // Changing velocity depending on info
        float sqrDistance = (transform.position - info.hitPos).sqrMagnitude;
        sqrDistance = Mathf.Clamp(sqrDistance, 0f, sqrMaxDistance);
        float distancePercentage = sqrMaxDistance / sqrDistance;

        if (skeleton.boneDetachCurve != null)
        {
            distancePercentage = skeleton.boneDetachCurve.curve.Evaluate(distancePercentage);
        }

        Vector3 detachVelocity = info.shotForward * info.force * detachForceMultiplier * distancePercentage;

        rb.velocity = detachVelocity / rb.mass;

        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 dist = transform.position - info.hitPos;
        

        if (GetComponent<GrabbableObject>() != null)
        {
            SetGrabbable();
        }

        detached = true;
    }

    void KnockBack(ShotInfo info)
    {
        Vector3 knockBackVelocity = info.shotForward * info.force * knockbackForceMultiplier;

        rb.velocity = knockBackVelocity / rb.mass;
    }

    void SetGrabbable()
    {
        if (GetComponent<Collider>() != null)
        {
            gameObject.layer = grabLayer;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = grabLayer;
        }
    }
}
