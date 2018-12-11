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

    float maxDistance = 1f;
    float sqrMaxDistance;

    bool detached;

    float detachForceMultiplier = 0.25f;
    float knockbackForceMultiplier = 1f;

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

        // Changing velocity depending on info

        Vector3 detachVelocity = Vector3.zero;

        float sqrDistance = (transform.position - info.hitPos).sqrMagnitude;
        sqrDistance = Mathf.Clamp(sqrDistance, 0f, sqrMaxDistance);
        float distancePercentage = sqrMaxDistance / sqrDistance;

        if(skeleton.boneDetachCurve != null)
        {
            distancePercentage = skeleton.boneDetachCurve.curve.Evaluate(distancePercentage);
        }

        detachVelocity += info.shotForward * info.force * detachForceMultiplier * distancePercentage;

        //rb.AddForce(detachVelocity * 1000000f, ForceMode.Force); // Doesn't seem to do anything
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
        // TODO: Implement behaviour if seperated limb is shot again
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
