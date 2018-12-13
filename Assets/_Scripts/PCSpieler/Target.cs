using UnityEngine;

public class Target : MonoBehaviour
{
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
                //set an particleEffect later
            }
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}