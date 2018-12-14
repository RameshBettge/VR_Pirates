using UnityEngine;

public class Cannonball : MonoBehaviour
{
    //public GameObject explosionEffect,    when we have one
    public float radius = 5f;
    public float damage = 10f;

    float force = 150f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " Has triggered ball");
        Explode();
    }

    void Explode()
    {
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearByObject in colliders)
        {
            IDamageable damageable = GetComponent<IDamageable>();

            if (damageable == null && nearByObject.transform.parent != null)
            {
                damageable = nearByObject.transform.parent.GetComponent<IDamageable>();
            }

            if(damageable != null)
            {
                Debug.Log(nearByObject.name + " hit by canooon " + nearByObject.transform.parent.name);

                Vector3 dir = (nearByObject.transform.position - transform.position).normalized;

                ShotInfo info = new ShotInfo(transform.position, dir, force, 100, 3);
            }
        }

        Destroy(gameObject);
    }
}