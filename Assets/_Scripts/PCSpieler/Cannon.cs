using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float shootForce;
    public GameObject cannonBallPrefab;
    public GameObject player;
    public Transform cannonSpawn;

    private Vector3 rot;

    GameObject childObject;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    public void Fire()
    {
        GameObject cannonBall = Instantiate(cannonBallPrefab, cannonSpawn.transform.position, cannonSpawn.transform.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(-cannonSpawn.transform.right * shootForce, ForceMode.VelocityChange);
        //-cannonSpawn.transform.right, because pivot of object is twisted
    }

    public void ActivateCannon()
    {
        childObject = GameObject.Find("CannonHolder");
        childObject.transform.parent = GameObject.Find("WeaponHolder").transform;
    }

    public void DeactivateCannon()
    {
        childObject = GameObject.Find("CannonHolder");
        childObject.transform.parent = GameObject.Find("Schiff [geunwrapet]").transform;
    }
}