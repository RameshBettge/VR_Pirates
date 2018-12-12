using UnityEngine;

public class Shooting : MonoBehaviour
{
    public int damage = 10;
    public float range = 8000f;
    public float impactForce = 100f;
    public float timeBetweenShots = 2f;

    public Camera cam;

    float timestamp;

    public Recoil recoilScript;

    void Update()
    {
        if (Time.time >= timestamp && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.collider.name);

            IDamageable damageable = (IDamageable)hit.collider.GetComponent(typeof(IDamageable));

            ShotInfo info = new ShotInfo(hit.point, transform.forward, impactForce, damage);

            Transform parent = hit.collider.transform.parent;
            if (damageable == null && parent != null)
            {
                damageable = (IDamageable)parent.GetComponent(typeof(IDamageable));
            }

            if (damageable != null)
            {
                damageable.TakeDamage(info);
            }

            //EnemyBehaiviour enemy = hit.transform.GetComponent<EnemyBehaiviour>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage);
            //}

            //if (hit.rigidbody != null)
            //{
            //    hit.rigidbody.AddForce(-hit.normal * impactForce);
            //}
        }
        timestamp = Time.time + timeBetweenShots;
        recoilScript.isRecoiling = true;
    }
}