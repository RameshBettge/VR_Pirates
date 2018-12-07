using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    float speed = 10;

    Vector3 lastPos;

    RaycastHit hit;

    float lifeTime = 10f;
    float timer;

    private void Awake()
    {
        hit = new RaycastHit();
    }

    void Update()
    {
        if(timer > lifeTime)
        {
            Destroy(gameObject);
            Destroy(this);
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        // Check for collision
        if (lastPos != Vector3.zero)
        {
            Vector3 dir = transform.position - lastPos;
            if (Physics.Raycast(transform.position, dir, out hit, mask))
            {
                // TODO: check if hit object is an enemy
                DetachableBone bone = hit.collider.transform.parent.GetComponent<DetachableBone>();
                if(bone == null)
                {
                    bone = hit.collider.transform.GetComponent<DetachableBone>();
                }

                if(bone != null)
                {
                    ShotInfo info = new ShotInfo(hit.point, transform.forward, 1f, 10);

                    Debug.Log("Shot hit: " + hit.collider.transform.parent.name);
                }
                else
                {
                    Debug.Log("Hit: " + hit.collider.name);
                }


                // TODO: put bullet into pool instead
                Destroy(gameObject);
                Destroy(this);
            }

        }

        lastPos = transform.position;

        timer += Time.deltaTime;
    }
}
