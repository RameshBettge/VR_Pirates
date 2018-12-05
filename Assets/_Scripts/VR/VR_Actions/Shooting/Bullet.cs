using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
            if (Physics.Raycast(transform.position, dir, out hit))
            {
                // TODO: check if hit object is an enemy

                // TODO: put bullet into pool instead
                Destroy(gameObject);
                Destroy(this);
            }

        }

        lastPos = transform.position;

        timer += Time.deltaTime;
    }
}
