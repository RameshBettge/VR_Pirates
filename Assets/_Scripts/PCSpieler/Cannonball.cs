using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public GameObject explosionEffect;
    public float radius = 5f;
    public float damage = 10f;

    void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearByObject in colliders)
        {
            EnemyBehaiviour enemy = nearByObject.GetComponent<EnemyBehaiviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}