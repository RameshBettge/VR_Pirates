using UnityEngine;

public class Mast : MonoBehaviour
{
    public float health = 2000;

    void OnTriggerEnter(Collider other)
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
            health -= 20;
            if (health <= 0)
            {
                GameObject.Find("SkelotonFinal-Animated PREFAB(Clone)").SendMessage("Idle");
                GameObject.Find("SkelotonFinal-Animated PREFAB 1(Clone)").SendMessage("Idle");
                GameObject.Find("SkelotonFinal-Animated PREFAB 2(Clone)").SendMessage("Idle");
                GameObject.Find("SkelotonFinal-Animated PREFAB 3(Clone)").SendMessage("Idle");
                Destroy(GameObject.FindWithTag("Target"));
            }
        }
    }
}