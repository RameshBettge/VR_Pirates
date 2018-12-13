using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IDamageable
{
    [Tooltip("If disabled, skeletons will only have an approximate hitbox when in boat.")]
    [SerializeField]
    bool exactCols = false;

    [SerializeField]
    float lifeTime = 30f;

    //[HideInInspector]
    public ScriptableCurve boneDetachCurve;

    [HideInInspector]
    public bool hasLostLimb = false;

    [HideInInspector]
    public bool destroyed = false;

    [HideInInspector]
    public bool boarded { get; private set; }

    DetachableBone[] bones;

    EnemyBehaiviour behaviour;

    int health = 5;

    private void Start()
    {
        boneDetachCurve = Resources.Load<ScriptableCurve>("ScriptableObjects/BoneDetachCurve");

        if(boneDetachCurve == null) { Debug.LogError("BoneDetachCurve couldn't be found!"); }

        bones = GetComponentsInChildren<DetachableBone>();

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].skeleton = this;
            bones[i].lifeTime = lifeTime;
        }

        Limb[] limbs = GetComponentsInChildren<Limb>();
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].skeleton = this;
        }

        if (exactCols)
        {
            gameObject.layer = LayerMask.NameToLayer("BulletIgnore");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Skeleton");

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].SetCols(false);
            }
        }

        behaviour = GetComponent<EnemyBehaiviour>();

        // TODO: Remove null check when prefab is done
        if (behaviour != null)
        {
            behaviour.enabled = false;
        }
    }

    public void OnBoarding()
    {
        boarded = true;

        if (!exactCols)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].SetCols(true);
            }
        }
    }


    public void TakeDamageFromBone(ShotInfo info, bool bodyShot = false)
    {
        health -= info.damage;

        if (health < 1)
        {
            Die(info);
        }
    }

    private void Die(ShotInfo info)
    {
        // TODO: Make sure all equipped weapons are detached! Stop Celina's logic
        if (destroyed) { return; }

        for (int i = 0; i < bones.Length; i++)
        {
            // TODO: Despawn bones
            bones[i].Detach(info);
        }

        destroyed = true;

        if (behaviour != null && boarded)
        {

            behaviour.Die();
        }
        else
        {
            // TODO: Check if destroying is save and not despawning loose bones/items
            Destroy(this);
            Destroy(gameObject);
        }
    }

    // Only used for sniper
    public void TakeDamage(ShotInfo info)
    {
        TakeDamageFromBone(info);
    }
}
