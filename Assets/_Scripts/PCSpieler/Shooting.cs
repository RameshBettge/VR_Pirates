using UnityEngine;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;

    [Tooltip("How far the shot direction should be lerped to Vector3.up")]
    [SerializeField]
    [Range(0, 1)]
    float upwardsKnockbackModifier = 0.25f;

    [SerializeField]
    float knockbackFalloffDistance = 1f;

    public int damage = 10;
    public float range = 3000f;
    public float impactForce = 300f;
    public float timeBetweenShots = 2f;

    public Camera cam;

    float timestamp;

    public Recoil recoilScript;

    void Update()
    {
        if (Time.timeSinceLevelLoad >= timestamp && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask))
        {
            //Debug.Log(hit.collider.gameObject.name);

            IDamageable damageable = (IDamageable)hit.collider.GetComponent(typeof(IDamageable));

            //Vector3 knockbackDir =Vector3.Lerp (transform.forward, transform.up, upwardsKnockbackModifier);
            Vector3 knockbackDir = cam.transform.forward;
            ShotInfo info = new ShotInfo(hit.point, knockbackDir, impactForce, damage, knockbackFalloffDistance);

            Transform parent = hit.collider.transform.parent;
            if (damageable == null && parent != null)
            {
                damageable = (IDamageable)parent.GetComponent(typeof(IDamageable));
            }

            if (damageable != null)
            {
                damageable.TakeDamage(info);
            }
        }
        timestamp = Time.timeSinceLevelLoad + timeBetweenShots;
        recoilScript.isRecoiling = true;
    }
}