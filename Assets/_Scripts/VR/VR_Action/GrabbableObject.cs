using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: disable collider when grabbed until directly after it is released.


[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour
{
    [SerializeField]
    bool activeOnStart = true;

    [SerializeField]
    float maxVelocity = 5f;
    [SerializeField]
    float velocityFactor = 4f;


    [SerializeField]
    float extraGravity = 10f;

    [HideInInspector]
    public bool isGrabbed;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = activeOnStart;

        rb.useGravity = false;
    }

    void Update()
    {
        if (rb.isKinematic) { return; }

        AddGravity();
    }

    private void AddGravity()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);
    }

    public void OnGrab()
    {
        rb.isKinematic = true;
        isGrabbed = true;
    }
    public void OnRelease(GrabData data, float deltaTime)
    {
        rb.isKinematic = false;
        isGrabbed = false;

        Vector3 averageVelocity = data.AverageMovement;

        //lastFrameMovement = data.hand.TransformDirection(lastFrameMovement);



        Vector3 velocity = averageVelocity;

        print(velocity.magnitude);
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        rb.velocity = velocity * velocityFactor;


        // TODO: apply angular velocity to thrown object.

        //Quaternion angularVelocity = Quaternion.FromToRotation(data.lastForward, data.hand.forward);
        //rb.angularVelocity = angularVelocity.eulerAngles / deltaTime;
    }
}
