using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Rigidbody))]
public class DetachableBone : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public float lifeTime;

    float despawnTime;

    [HideInInspector]
    public Limb limb;

    [HideInInspector]
    public Skeleton skeleton;

    Rigidbody rb;
    Collider[] cols;
    Collider[] Cols
    {
        get
        {
            if (cols == null)
            {
                cols = GetComponentsInChildren<Collider>(true);

            }
            return cols;
        }
    }
        

    int grabLayer;

    bool detached;

    float detachForceMultiplier = 0.22f;
    float knockbackForceMultiplier = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        grabLayer = LayerMask.NameToLayer("Grabbable");

        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].enabled = true;
        }

        if (Cols.Length < 1)
        {
            Debug.LogWarning(name + " (DetachableBone) has no collider!");
        }
    }

    private void Update()
    {
        if (!detached) { return; }
        if(Time.time >= despawnTime || transform.position.y < -10f)
        {
            Destroy(gameObject);
            // Not destroying this script because it should only cause harm if it was null
        }
    }

    public void SetCols(bool active)
    {
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].gameObject.SetActive(active);
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
            skeleton.TakeDamageFromBone(info, true);
        }
        else
        {
            limb.TakeDamage(info);
        }
    }

    public void Detach(ShotInfo info)
    {
        if (detached)
        {
            return;
        }

        // Changing velocity depending on info
        

        float distancePercentage = info.GetDistancePercentage(transform);

        if (skeleton.boneDetachCurve != null)
        {
            distancePercentage = skeleton.boneDetachCurve.curve.Evaluate(distancePercentage);
        }

        Vector3 detachVelocity = info.shotForward * info.force * detachForceMultiplier * distancePercentage;

        rb.velocity = detachVelocity / rb.mass;

        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;


        if (GetComponent<GrabbableObject>() != null)
        {
            SetGrabbable();
        }

        detached = true;

        despawnTime = Time.time + lifeTime;
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
