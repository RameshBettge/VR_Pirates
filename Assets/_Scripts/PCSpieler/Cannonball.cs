using UnityEngine;

public class Cannonball : MonoBehaviour
{
    //public GameObject explosionEffect,    when we have one
    [SerializeField]
    LayerMask mask;

    float radius = 5f;
    int damage = 1000;

    float force = 150f;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + " Has triggered ball. Parent is: " + other.transform.parent.name);
        //string layer = LayerMask.LayerToName(other.gameObject.layer);

        float dist = (transform.position - other.ClosestPoint(transform.position)).magnitude;

        Explode();
    }

    void Explode()
    {
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask);

        foreach (Collider nearByObject in colliders)
        {
            IDamageable damageable = GetComponent<IDamageable>();

            if (damageable == null && nearByObject.transform.parent != null)
            {
                damageable = nearByObject.transform.parent.GetComponent<IDamageable>();
            }

            if(damageable != null)
            {
                //Debug.Log(nearByObject.name + " hit by canooon " + nearByObject.transform.parent.name);

                Vector3 dir = (nearByObject.transform.position - transform.position).normalized;

                ShotInfo info = new ShotInfo(transform.position, dir, force, damage, 3);

                damageable.TakeDamage(info);
            }
        }

        Destroy(gameObject);
    }
}