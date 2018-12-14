using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform bulletSpawn;

    [SerializeField]
    Transform Trigger;

    [SerializeField]
    float activatedTriggerRotation = -20f;

    [SerializeField]
    float timeUntilDespawn = 5f;

    [HideInInspector]
    public Holster holster;

    [HideInInspector]
    public bool discarded = false;

    [SerializeField]
    int magazineSize = 5;

    [HideInInspector]
    public Transform ship;


    int bulletsInMagazine;

    float despawnTime;

    private void Awake()
    {
        bulletsInMagazine = magazineSize;
    }

    public void OnGrab(Transform ship)
    {
        this.ship = ship;

        discarded = false;
        holster.OnDrawPistol();

    }

    public void Discard()
    {
        discarded = true;
        despawnTime = Time.timeSinceLevelLoad + timeUntilDespawn;

    }

    private void Update()
    {
        if(!discarded) { return; }

        if(Time.timeSinceLevelLoad >= despawnTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetTriggerRotation(float input)
    {
        Vector3 localEuler = Trigger.localEulerAngles;
        localEuler.x = activatedTriggerRotation * input;

        Trigger.localEulerAngles = localEuler;
    }

    public void Shoot()
    {
        if(bulletsInMagazine < 1)
        {
            //Debug.Log("Magazine empty.");
            return;
        }


        Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation, ship);

        bulletsInMagazine--;
        //Debug.Log("pew! bullets left: " + bulletsInMagazine);
    }
}

public struct ShotInfo
{
    public Vector3 hitPos;
    public Vector3 shotForward;
    public float force;
    public int damage;
    float sqrMaxDistance;


    public ShotInfo(Vector3 hitPos, Vector3 shotForward, float force, int damage, float knockbackInfluenceDistance)
    {
        this.hitPos = hitPos;
        this.shotForward = shotForward.normalized;
        this.force = force;
        this.damage = damage;
        sqrMaxDistance = knockbackInfluenceDistance;
    }

    public float GetDistancePercentage(Transform t)
    {
        float sqrDistance = (t.position - hitPos).sqrMagnitude;
        sqrDistance = Mathf.Clamp(sqrDistance, 0.001f, sqrMaxDistance);

        // if distance is high -> percentage is low.
        float distancePercentage = 1 - (sqrDistance / sqrMaxDistance);

        return distancePercentage;
    }
}
