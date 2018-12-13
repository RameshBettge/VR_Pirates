using UnityEngine;

public class CameraSwitching : MonoBehaviour
{
    public Camera thirdPersonCam;
    public Camera thirdPersonCam2;
    public Camera firstPersonCam;

    public void Start()
    {
        thirdPersonCam.enabled = true;
        thirdPersonCam2.enabled = false;
        firstPersonCam.enabled = false;
    }

    //Camera View BucketThrow
    public void ThirdPerson()
    {
        thirdPersonCam.enabled = true;
        thirdPersonCam2.enabled = false;
        firstPersonCam.enabled = false;
    }

    //Camera View Cannon
    public void ThirdPerson2()
    {
        thirdPersonCam.enabled = false;
        thirdPersonCam2.enabled = true;
        firstPersonCam.enabled = false;
    }

    //Camera View Sniper
    public void FirstPerson()
    {
        thirdPersonCam.enabled = false;
        thirdPersonCam2.enabled = false;
        firstPersonCam.enabled = true;
    }
}