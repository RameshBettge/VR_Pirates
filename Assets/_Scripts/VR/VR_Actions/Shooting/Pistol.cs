using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: public Shoot()

public class Pistol : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform bulletSpawn;



    int magazineSize = 50;

    int bulletsInMagazine;

    private void Awake()
    {
        bulletsInMagazine = magazineSize;
    }

    public void Shoot()
    {
        if(bulletsInMagazine < 1)
        {
            //Debug.Log("Magazine empty.");
            return;
        }


        Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

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


    public ShotInfo(Vector3 hitPos, Vector3 shotForward, float force, int damage)
    {
        this.hitPos = hitPos;
        this.shotForward = shotForward.normalized;
        this.force = force;
        this.damage = damage;
    }
}
