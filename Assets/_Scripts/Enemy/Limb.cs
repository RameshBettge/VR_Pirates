using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    [HideInInspector]
    public Skeleton skeleton;

    [HideInInspector]
    public bool destroyed = false;

    DetachableBone[] bones;

    int health = 2;

    float debug = 0;

    private void Awake()
    {
        bones = GetComponentsInChildren<DetachableBone>();

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            Debug.LogWarning(name + "(Limb) has a renderer attached. A limb should always be the bone of a rig!");
        }

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].limb = this;
        }
    }

    private void Update()
    {
        //// DEBUGGING

        //if (Input.GetButtonDown("Jump"))
        //{
        //    destroyed = false;
        //    ShotInfo testInfo = new ShotInfo(Vector3.zero, Vector3.right, 1f, 1);
        //    LoseLimb(testInfo);
        //}
    }

    public void TakeDamage(ShotInfo info)
    {
        if (destroyed) { return; }

        skeleton.TakeDamage(info);

        health -= info.damage;

        if (health < 1 && !skeleton.hasLostLimb)
        {
            LoseLimb(info);
            skeleton.hasLostLimb = true;
        }
    }

    public void LoseLimb(ShotInfo info)
    {
        if (destroyed) { return; }

        // unparent all bones. set their rigidbody.isKinematic to false. Add force to send them flying
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].Detach(info);
        }

        destroyed = true;
    }
}
