using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject fireEffect;
    public GameObject waterSplash;
    public Transform ship;
    public float health = 500;
    float burnThreshold = 400f;
    bool walkingWasTriggered = false;

    bool isBurning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isBurning)
        {
            health -= 10f;
        }

        if (other.gameObject.tag == "Weapon")
        {
            health -= 10;
            if (health <= burnThreshold && !walkingWasTriggered)
            {
                isBurning = true;

                walkingWasTriggered = true;
                EnemyBehaiviour behaviour = other.GetComponentInParent<EnemyBehaiviour>();
                if(behaviour != null)
                {
                    behaviour.Walking();
                }

                MoveOnPath move = other.GetComponentInParent<MoveOnPath>();
                if(move != null)
                {
                    // Why 2.5f?
                    move.speed = 2.5f;
                }
                Instantiate(fireEffect, transform.position, transform.rotation, ship);

                burnThreshold -= 100f;
            }
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "Bucket")
        {
            Bucket bucket = other.GetComponent<Bucket>();
            if (bucket == null) { return; }
            if (!bucket.filled)
            {
                return;
            }
            walkingWasTriggered = false;

            Instantiate(waterSplash, transform.position, transform.rotation, ship);
            isBurning = false;
        }
    }
}