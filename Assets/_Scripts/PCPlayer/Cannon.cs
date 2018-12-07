using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float shootForce;
    public GameObject cannonBallPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject cannonBall = Instantiate(cannonBallPrefab, transform.position, transform.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootForce, ForceMode.VelocityChange);
    }
}