using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject fireEffect;
    public GameObject waterSplash;
    public float health = 500;
    bool walkingWasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            health -= 10;
            if (health <= 400 && !walkingWasTriggered)
            {
                walkingWasTriggered = true;
                other.GetComponentInParent<EnemyBehaiviour>().Walking();
                other.GetComponentInParent<MoveOnPath>().speed = 2.5f;
                Instantiate(fireEffect, transform.position, transform.rotation);
            }
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "Bucket")
        {
            Instantiate(waterSplash, transform.position, transform.rotation);
        }
    }
}