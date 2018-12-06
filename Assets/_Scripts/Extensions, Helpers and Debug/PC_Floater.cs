using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PC_Floater : MonoBehaviour {

    Transform head;
    float headTiltSpeed = 100f;
    public float currentHeadRotation;
    public float convertedRotation;
    float moveForce = 1800f;
    float maxSpeed = 7f;
    float sqrMaxSpeed;

    public float velocity;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        head = transform.GetComponentInChildren<Camera>().transform;

        sqrMaxSpeed = maxSpeed * maxSpeed;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    void Rotate()
    {
        float maxUpTilt = 90f;
        float maxDownTilt = 85f;

        float yRotation = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, yRotation * headTiltSpeed * Time.deltaTime);


        float xRotation = -Input.GetAxis("Mouse Y");

        currentHeadRotation += xRotation * headTiltSpeed * Time.deltaTime;
        currentHeadRotation = Mathf.Clamp(currentHeadRotation, -maxUpTilt, maxDownTilt);

        convertedRotation = currentHeadRotation;
        if (convertedRotation < 0f)
        {
            convertedRotation += 360f;
        }

        head.localEulerAngles = Vector3.right * convertedRotation;
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputVector = new Vector3(h, 0f, v).normalized;

        Vector3 movementVector = head.forward * v + head.right * h;

        Vector3 planarVelocity = rb.velocity;
        float sqrMag = planarVelocity.sqrMagnitude;

        if (sqrMag < sqrMaxSpeed)
        {
            rb.AddForce(movementVector * moveForce * Time.deltaTime);
        }

        if (movementVector.sqrMagnitude < 0.01f)
        {
            rb.drag = 4f;
        }
        else
        {
            rb.drag = 0.7f;
        }
    }
}
