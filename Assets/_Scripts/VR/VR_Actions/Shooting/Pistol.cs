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



    int magazineSize = 5;

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
