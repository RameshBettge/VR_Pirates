using UnityEngine;

public class EnemyBehaiviour : MonoBehaviour
{
    public MoveOnPath moveScript;

    Animator anim;
    int id_attack_trigger;
    int id_death_trigger;
    int id_walking_trigger;
    int id_idle_trigger;

    public float health = 50f;

    void Start()
    {
        anim = GetComponent<Animator>();
        id_attack_trigger = Animator.StringToHash("attack");
        id_death_trigger = Animator.StringToHash("death");
        id_walking_trigger = Animator.StringToHash("walking");
        id_idle_trigger = Animator.StringToHash("idle");
        moveScript.enabled = true;
    }

    public void Attack()
    {
        anim.SetTrigger(id_attack_trigger);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }


    // TODO: Call this on Skeleton.Die()
    public void Die()
    {
        anim.SetTrigger(id_death_trigger);
        Destroy(this, 1f);
        Destroy(gameObject, 1f);
        if (moveScript.enabled == true)
        {
            moveScript.enabled = false;
        }
    }

    public void Walking()
    {
        anim.SetTrigger(id_walking_trigger);
    }

    public void Idle()
    {
        anim.SetTrigger(id_idle_trigger);
    }
}