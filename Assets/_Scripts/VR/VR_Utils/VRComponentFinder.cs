using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;


public class VRComponentFinder : MonoBehaviour
{
    public VRInputLookup lookup;

    public Transform Head { get; private set; }
    public Transform RightHand { get; private set; }
    public Transform LeftHand { get; private set; }


    private void Awake()
    {
        TrackedPoseDriver[] drivers = GetComponentsInChildren<TrackedPoseDriver>();

        for (int i = 0; i < drivers.Length; i++)
        {
            switch (drivers[i].poseSource)
            {
                case TrackedPoseDriver.TrackedPose.Head:
                    Head = drivers[i].transform;
                    break;
                case TrackedPoseDriver.TrackedPose.LeftPose:
                    LeftHand = drivers[i].transform;
                    break;
                case TrackedPoseDriver.TrackedPose.RightPose:
                    RightHand = drivers[i].transform;
                    break;
                default:
                    break;
            }
        }

        if(Head == null)
        {
            LogMissingError("Head");
        }
        if (RightHand == null)
        {
            LogMissingError("Right Hand");
        }
        if (LeftHand == null)
        {
            LogMissingError("Left Hand");
        }
    }

    void LogMissingError(string partname)
    {
        Debug.LogError("VR component '" + partname + "' could not be found.");
    }
}
