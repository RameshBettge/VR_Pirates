using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float shootForce;
    public GameObject cannonBallPrefab;
    public GameObject player;
    public Transform cannon;

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
        GameObject cannonBall = Instantiate(cannonBallPrefab, cannon.transform.position, cannon.transform.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(-transform.up * shootForce, ForceMode.VelocityChange);
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