using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 500;
    bool walkingWasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy1")
        {
            other.GetComponent<MoveOnPath>().speed = 0f;
            GameObject.Find("SkelotonFinal-Animated PREFAB(Clone)").SendMessage("Attack");
        }
        if (other.gameObject.tag == "Enemy2")
        {
            other.GetComponent<MoveOnPath>().speed = 0f;
            GameObject.Find("SkelotonFinal-Animated PREFAB 1(Clone)").SendMessage("Attack");
        }
        if (other.gameObject.tag == "Enemy3")
        {
            other.GetComponent<MoveOnPath>().speed = 0f;
            GameObject.Find("SkelotonFinal-Animated PREFAB 2(Clone)").SendMessage("Attack");
        }
        if (other.gameObject.tag == "Enemy4")
        {
            other.GetComponent<MoveOnPath>().speed = 0f;
            GameObject.Find("SkelotonFinal-Animated PREFAB 3(Clone)").SendMessage("Attack");
        }

        if (other.gameObject.tag == "Weapon")
        {
            health -= 10;
            if (health <= 400 && !walkingWasTriggered)
            {
                walkingWasTriggered = true;
                other.GetComponentInParent<EnemyBehaiviour>().Walking();
                other.GetComponentInParent<MoveOnPath>().speed = 2.5f;
                //hier Particle Effect einfügen
            }
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}