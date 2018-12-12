using UnityEngine;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    public int damage = 10;
    public float range = 3000f;
    public float impactForce = 100f;
    public float timeBetweenShots = 2f;

    public Camera cam;

    float timestamp;

    public Recoil recoilScript;

    public List<Vector3> debugs = new List<Vector3>();

    private void OnDrawGizmos()
    {
        if(debugs == null) { return; }

        for (int i = 0; i < debugs.Count; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(debugs[i], 0.75f);
        }
    }

    void Update()
    {
        debugs.Clear();
        float debugIncrement = 20f;
        float distance = debugIncrement;
        while(distance <= range)
        {
            debugs.Add(cam.transform.position + cam.transform.forward * distance);
            distance += debugIncrement;
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward * range, Color.magenta);
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