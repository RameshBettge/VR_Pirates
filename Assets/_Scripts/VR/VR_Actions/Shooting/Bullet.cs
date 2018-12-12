using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    [SerializeField]
    float speed = 10f;

    // TODO: Change force to a higher value - sniper needs around 100 to look good.
    //       (Please don't forget to adjust value on perfab and not here ;) )
    [SerializeField]
    float force = 1f;

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


            if (Physics.Raycast(ray, out hit, dir.magnitude, mask))
            {
                IDamageable damageable = (IDamageable)hit.collider.GetComponent(typeof(IDamageable));

                ShotInfo info = new ShotInfo(hit.point, transform.forward, force, damage, knockbackFalloffDistance);

                Transform parent = hit.collider.transform.parent;
                if (damageable == null && parent != null)
                {
                    damageable = (IDamageable)parent.GetComponent(typeof(IDamageable));
                }

                if (damageable != null)
                {
                    damageable.TakeDamage(info);
                }


                // TODO: Remove the commented out lines - they fell out of use after IDamageable was implemented but are kept jsut to be sure.
                //DetachableBone bone = hit.collider.transform.parent.GetComponent<DetachableBone>();
                //if (bone == null)
                //{
                //    bone = hit.collider.transform.GetComponent<DetachableBone>();
                //}

                //if (bone != null)
                //{

                //    bone.TakeDamage(info);
                //    // apply damage to bone
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
