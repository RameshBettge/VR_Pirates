﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class Bone : MonoBehaviour
//{
//    [SerializeField]
//    int damageMultipler = 1;

//    [HideInInspector]
//    public LimbHealth limb;


//    public void OnBulletHit(ShotInfo info)
//    {
//        info.damage *= damageMultipler;

//        if (limb != null)
//        {
//            limb.TakeDamage(info);
//        }
//    }
//}

public struct ShotInfo
{
    public Vector3 hitPos;
    public Vector3 dir;
    public float force;
    public int damage;


    public ShotInfo(Vector3 hitPos, Vector3 dir, float force, int damage)
    {
        this.hitPos = hitPos;
        this.dir = dir.normalized;
        this.force = force;
        this.damage = damage;
    }
}