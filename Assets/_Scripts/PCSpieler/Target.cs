using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject fireEffect;
    public GameObject waterSplash;
    public Transform ship;
    float health = 800;
    float burnThreshold = 400f;
    float burnThresholdIncrement = 100f;

    bool walkingWasTriggered = false;

    bool isBurning = false;

    GameObject particleFire;

    private void Awake()
    {
        StartBurn();
    }

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
                StartBurn();

                EnemyBehaiviour behaviour = other.GetComponentInParent<EnemyBehaiviour>();
                if (behaviour != null)
                {
                    behaviour.Walking();
                }

                MoveOnPath move = other.GetComponentInParent<MoveOnPath>();
                if (move != null)
                {
                    // Why 2.5f?
                    move.speed = 2f;
                }
            }
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void StartBurn()
    {
        isBurning = true;

        walkingWasTriggered = true;
       
        particleFire = Instantiate(fireEffect, transform.position, transform.rotation, ship);

        burnThreshold -= burnThresholdIncrement;
    }

    public void Extinguish()
    {
        if (!isBurning) { return; }

        walkingWasTriggered = false;

        Destroy(particleFire);

        Instantiate(waterSplash, transform.position, transform.rotation, ship);
        isBurning = false;
    }
}