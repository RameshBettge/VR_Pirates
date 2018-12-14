using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float shootForce;
    public GameObject cannonBallPrefab;
    public GameObject player;
    public Transform ship;
    public Transform cannonSpawn;

    private Vector3 rot;

    [HideInInspector]
    public int stock = 0;

    GameObject childObject;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (stock > 0)
            {
                Fire();
                stock--;
            }
        }
    }

    public void Fire()
    {
        GameObject cannonBall = Instantiate(cannonBallPrefab, cannonSpawn.transform.position, cannonSpawn.transform.rotation, ship);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

        Vector3 randomRot = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            randomRot[i] = Random.Range(-500f, 500f);
        }

        rb.AddTorque(randomRot);

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