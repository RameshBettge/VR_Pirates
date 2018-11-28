using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGrab : MonoBehaviour
{
    [SerializeField]
    float grabThreshold = 0.5f;
    [SerializeField]
    float releaseThreshold = 0.25f;

    [SerializeField]
    float grabRange = 1;

    [SerializeField]
    LayerMask mask;

    VRComponentFinder finder;
    VRInputLookup vrInput;

    GrabData leftData = new GrabData();
    GrabData rightData = new GrabData();

    public float debugF;

    private void Start()
    {
        finder = GetComponent<VRComponentFinder>();
        vrInput = finder.lookup;

        leftData.controller = vrInput.Left;
        leftData.hand = finder.LeftHand;

        rightData.controller = vrInput.Right;
        rightData.hand = finder.RightHand;
    }

    private void Update()
    {
        CheckInput(leftData);
        CheckInput(rightData);
    }

    void CheckInput(GrabData data)
    {
        float input = data.controller.Grab.Value;
        debugF = input;
        if (data.isGrabbing)
        {
            if (input < releaseThreshold)
            {
                Release(data);
            }

            data.SetVelocity(data.hand.localPosition, Time.deltaTime);
            data.lastForward = data.hand.forward;
        }
        else
        {
            if (input > grabThreshold)
            {
                Grab(data);
            }
        }
    }

    void Grab(GrabData data)
    {
        Collider[] cols = Physics.OverlapSphere(data.hand.position, grabRange, mask);

        Transform grabbed = null;
        float leastDistance = Mathf.Infinity;

        GrabbableObject gObject = null;

        for (int i = 0; i < cols.Length; i++)
        {
            gObject = cols[i].GetComponent<GrabbableObject>();
            if (gObject == null)
            {
                Debug.LogError("There was no GrabbableObject-component found on " + 
                    cols[i].gameObject.name +
                    ", even though it is on layer specified by mask.");
                return;
            }
            else if (gObject.isGrabbed)
            {
                continue;
            }

            Vector3 handPos = data.hand.position;
            Vector3 closest = cols[i].ClosestPoint(handPos);
            float sqrDist = (closest - handPos).sqrMagnitude;

            if (sqrDist < leastDistance)
            {
                leastDistance = sqrDist;
                grabbed = cols[i].transform;
            }
        }

        if (grabbed == null) // nothing to be grabbed
        {
            return;
        }
        gObject = grabbed.GetComponent<GrabbableObject>();




        grabbed.transform.parent = data.hand;
        grabbed.localPosition = Vector3.zero;

        gObject.OnGrab();

        data.grabbedObject = gObject;

        data.isGrabbing = true;
    }

    void Release(GrabData data)
    {
        data.grabbedObject.transform.parent = null;


        data.grabbedObject.OnRelease(data, Time.deltaTime);
        data.grabbedObject = null;

        data.isGrabbing = false;
        data.ResetVelocity();

    }
}

public class GrabData
{
    public VRController controller;
    public Transform hand;
    public bool isGrabbing;
    public GrabbableObject grabbedObject;

    Vector3 lastPos;
    int maxVelocitySamples = 10;
    int velocitySamples = 0;
    public Vector3 lastForward;

    public Vector3 AverageMovement { get; private set; }

    public void SetVelocity(Vector3 currentPos, float deltaTime)
    {
        Vector3 difference = (currentPos - lastPos)/deltaTime;

        //Debug.Log(difference + " difference | pos " + currentPos + " lastpos: " +lastPos);


        if (!(velocitySamples >= maxVelocitySamples))
        {
            velocitySamples++;
        }

        if (velocitySamples == 1)
        {
            AverageMovement = difference;
        }
        else
        {
            AverageMovement = AverageMovement / (velocitySamples) * (velocitySamples - 1);
            AverageMovement += (difference / velocitySamples);

            //Debug.Log(difference + " difference | average " + AverageMovement);
        }

        lastPos = currentPos;
    }

    public void ResetVelocity()
    {
        velocitySamples = 0;
        AverageMovement = Vector3.zero;
    }
}