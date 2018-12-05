using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: call pistol.shoot if index-axis is pressed

public class VRGrab : MonoBehaviour
{
    [SerializeField]
    float grabThreshold = 0.25f;
    [Tooltip("When the grabbed object is dropped")]
    [SerializeField]
    float hardReleaseThreshold = 0.2f;
    [Tooltip("How far the hand has to be open until another grab can be attempted")]
    [SerializeField]
    float softReleaseThreshold = 0.5f;
    [SerializeField]
    float shootThreshold = 0.8f;
    [SerializeField]
    float shootResetThreshold = 0.2f;

    [SerializeField]
    float grabRange = 1;

    [Tooltip("How many of the last frames are taken into consideration " +
        "when calculating thrown object's velocity and angular velocity.")]
    [SerializeField]
    int maxVelocitySamples = 12;


    [SerializeField]
    LayerMask mask;

    VRComponentFinder finder;
    VRInputLookup vrInput;

    public GrabData leftData;
    public GrabData rightData;


    private void Start()
    {
        finder = GetComponent<VRComponentFinder>();
        vrInput = finder.lookup;

        leftData = new GrabData(maxVelocitySamples);
        rightData = new GrabData(maxVelocitySamples);

        leftData.controller = vrInput.Left;
        rightData.controller = vrInput.Right;

        leftData.hand = finder.LeftHand;
        rightData.hand = finder.RightHand;

        leftData.anims = finder.LeftHand.GetComponentsInChildren<Animator>();
        rightData.anims = finder.RightHand.GetComponentsInChildren<Animator>();

        leftData.isLeft = true;
    }

    private void Update()
    {
        CheckInput(leftData);
        CheckInput(rightData);
    }

    void CheckInput(GrabData data)
    {
        float grabInput = data.controller.Grab.Value;
        for (int i = 0; i < 2; i++)
        {
            data.anims[i].SetFloat("GrabValue", grabInput);
        }

        if (data.isGrabbing)
        {
            if (grabInput < hardReleaseThreshold)
            {
                Release(data);
            }

            Vector3 localForward = transform.InverseTransformDirection(data.hand.forward);

            data.SetVelocity(data.hand.localPosition, localForward, Time.deltaTime);
        }
        else if (data.hasAttemptedGrab)
        {
            if (grabInput < softReleaseThreshold)
            {
                data.hasAttemptedGrab = false;
            }
        }
        else
        {
            // Attempt to grab sth.
            if (grabInput > grabThreshold)
            {
                Grab(data);
            }
        }


        if (data.pistol != null)
        {
            float indexInput = data.controller.Index.Value;

            for (int i = 0; i < 2; i++)
            {
                data.anims[i].SetFloat("IndexValue", indexInput);
            }

            if (data.alreadyShot)
            {
                if (indexInput < shootResetThreshold)
                {
                    Debug.Log("ShootReset: " + indexInput);
                    data.alreadyShot = false;
                }
            }
            else if (indexInput > shootThreshold)
            {
                data.pistol.Shoot();
                Debug.Log("Shoot: " + indexInput);
                data.alreadyShot = true;
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
                Debug.LogError("There was no VRInputSetup-component found on " +
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
            data.hasAttemptedGrab = true;
            return;
        }

        gObject = grabbed.GetComponent<GrabbableObject>();

        grabbed.transform.parent = data.hand;
        grabbed.localPosition = Vector3.zero;

        gObject.OnGrab(data);

        data.grabbedObject = gObject;

        Pistol pistol = gObject.GetComponent<Pistol>();
        if (pistol != null)
        {
            data.pistol = pistol;

            for (int i = 0; i < 2; i++)
            {
                data.anims[i].SetBool("Pistol", true);
            }
        }
        else
        {
            // Set other bool in animator
        }

        data.isGrabbing = true;
    }

    void Release(GrabData data)
    {
        data.grabbedObject.transform.parent = null;


        data.grabbedObject.OnRelease(data, transform, Time.deltaTime);
        data.grabbedObject = null;

        data.isGrabbing = false;
        data.Reset();
    }
}

// TODO: Extract into seperate file
[System.Serializable]
public class GrabData
{
    public bool alreadyShot = true;
    public bool isLeft;

    public Animator[] anims;

    public VRController controller;
    public Transform hand;
    public bool isGrabbing;
    public bool hasAttemptedGrab;
    public GrabbableObject grabbedObject;

    public Pistol pistol;

    Vector3 lastPos;
    int maxVelocitySamples;
    public int velocitySamples = 0;
    Vector3 lastForward;

    public Vector3 AverageMovement;
    Vector3 averageRotation;
    public Vector3 AverageRotation
    {
        get
        {
            //Vector3 output = averageRotation / maxVelocitySamples / maxVelocitySamples * velocitySamples * velocitySamples;
            //Debug.Log("output: " + output + " - averageRotation: " + averageRotation + " - maxVelocitySamples: " + maxVelocitySamples + " - velocitySamples: " + velocitySamples);
            return averageRotation;
        }
        private set { averageRotation = value; }
    }

    public GrabData(int maxVelocitySamples)
    {
        this.maxVelocitySamples = maxVelocitySamples;
    }

    public void SetVelocity(Vector3 currentPos, Vector3 currentLocalForward, float deltaTime)
    {
        Vector3 difference = (currentPos - lastPos) / deltaTime;

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

        }

        lastPos = currentPos;


        // Rotation

        if (velocitySamples == 1)
        {
            AverageRotation = Vector3.zero;
        }
        else
        {
            AverageRotation = AverageRotation / velocitySamples * (velocitySamples - 1);
            Vector3 rot = Quaternion.FromToRotation(lastForward, currentLocalForward).eulerAngles;

            for (int i = 0; i < 3; i++)
            {
                if (rot[i] > 180f)
                {
                    rot[i] = (rot[i] - 360f);
                }
            }
            rot /= Time.deltaTime;
            rot /= velocitySamples;

            AverageRotation += rot;
        }

        lastForward = currentLocalForward;
    }

    public void Reset()
    {
        velocitySamples = 0;
        AverageMovement = Vector3.zero;
        AverageRotation = Vector3.zero;

        for (int i = 0; i < 2; i++)
        {
            anims[i].SetBool("Pistol", false);
        }
        // TODO: reset all other bools;

        pistol = null;
    }
}

