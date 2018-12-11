using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    float speed = 10f;

    [SerializeField]
    float force = 1f;

    [SerializeField]
    int damage = 1;

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


            if (Physics.Raycast(ray, out hit, dir.magnitude, mask))
            {
                // TODO: check if hit object is an enemy
                DetachableBone bone = hit.collider.transform.parent.GetComponent<DetachableBone>();
                if (bone == null)
                {
                    bone = hit.collider.transform.GetComponent<DetachableBone>();
                }

                if (bone != null)
                {
                    ShotInfo info = new ShotInfo(hit.point, transform.forward, force, damage);

                    bone.TakeDamage(info);
                    // apply damage to bone
                }


                //Debug.Log("Hit " + hit.collider.transform.parent.name + " with dir" + dir * 100f + " on frame " + debugInt);
                //Debug.Log("Hit " + hit.collider.transform.parent.name + " on frame " + debugInt);


                // TODO: put bullet into pool instead
                Destroy(gameObject);
                Destroy(this);
            }



        }

        lastPos = transform.position;

        timer += Time.deltaTime;
    }
}
