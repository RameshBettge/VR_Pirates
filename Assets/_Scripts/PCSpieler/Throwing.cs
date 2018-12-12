using UnityEngine;

public class Throwing : MonoBehaviour
{
    public GameObject grenadePrefab;
    public GameObject player;
    public Transform weaponHolder;

    public float grenadeDistance = 0.75f;

    private bool holdingGrenade = true;

    private GameObject grenade;

    Transform childGranate;
    public bool detachChild;

    float start;
    float end;
    public float force = 10f;

    void Start()
    {
        grenade = Instantiate(grenadePrefab, weaponHolder);
        grenade.transform.SetSiblingIndex(0);
    }

    void Update()
    {
        GrenadeThrow();
    }

    void GrenadeThrow()
    {
        if (holdingGrenade)
        {
            grenade.transform.position = player.transform.position + player.transform.forward * grenadeDistance + -player.transform.right * 0.6f + player.transform.up * 0.5f;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                start = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                end = Time.time;
                force = (end - start) * 1000;
                holdingGrenade = false;
                grenade.GetComponent<Rigidbody>().useGravity = true;
                grenade.GetComponent<Rigidbody>().AddForce(-player.transform.right * force);
                if (detachChild == true)
                {
                    grenade.transform.parent = null;
                    Destroy(grenade, 3.0f);
                }
                grenade = Instantiate(grenadePrefab, grenadePrefab.transform.position, grenadePrefab.transform.rotation, weaponHolder);
                grenade.transform.SetSiblingIndex(0);
                holdingGrenade = true;
            }
        }
    }
}