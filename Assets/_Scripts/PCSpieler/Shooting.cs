using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
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
            Debug.Log(hit.transform.name);

            EnemyBehaiviour enemy = hit.transform.GetComponent<EnemyBehaiviour>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
        timestamp = Time.time + timeBetweenShots;
        recoilScript.isRecoiling = true;
    }
}