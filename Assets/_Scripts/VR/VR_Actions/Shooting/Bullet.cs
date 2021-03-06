﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    float speed = 10f;

    [SerializeField]
    float force = 100f;

    [SerializeField]
    int damage = 1;

    [SerializeField]
    float knockbackFalloffDistance = 0.8f;

    Vector3 lastPos = Vector3.zero;

    RaycastHit hit;

    float lifeTime = 10f;
    float timer;

    int debugInt = 0;


    private void Awake()
    {
        hit = new RaycastHit();
    }

    void Update()
    {
        if (timer > lifeTime)
        {
            Destroy(gameObject);
            Destroy(this);
        }

        debugInt++;


        transform.position += transform.forward * speed * Time.deltaTime;

        // Check for collision
        if (lastPos != Vector3.zero)
        {
            Vector3 dir = transform.position - lastPos;

            Debug.DrawRay(transform.position, -dir, Color.red);

            Ray ray = new Ray(transform.position, -dir.normalized);


            if (Physics.Raycast(ray, out hit, dir.magnitude, mask, QueryTriggerInteraction.Ignore))
                {
                IDamageable damageable = (IDamageable)hit.collider.GetComponent(typeof(IDamageable));

                ShotInfo info = new ShotInfo(hit.point, transform.forward, force, damage, knockbackFalloffDistance);

                Transform parent = hit.collider.transform.parent;
                if (damageable == null && parent != null)
                {
                    damageable = (IDamageable)parent.GetComponent(typeof(IDamageable));
                }

                //Debug.Log("Bullet hit: " + hit.collider.name + " (child of " + hit.collider.transform.parent.name + ")");
                if (damageable != null)
                {
                    damageable.TakeDamage(info);
                }
                //else
                //{
                //    Debug.Log("Hit object isn't damageable!");
                //}


                // TODO: put bullet into pool instead
                Destroy(gameObject);
                Destroy(this);
            }



        }

        lastPos = transform.position;

        timer += Time.deltaTime;
    }
}
