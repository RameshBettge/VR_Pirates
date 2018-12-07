using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mast : MonoBehaviour
{
    public float health = 1000;

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
            health -= 20;
            if (health <= 0)
            {
                other.GetComponentInParent<EnemyBehaiviour>().Idle();
                Destroy(gameObject);
            }
        }
    }
}