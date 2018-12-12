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

    public void ThirdPerson()
    {
        thirdPersonCam.enabled = true;
        thirdPersonCam2.enabled = false;
        firstPersonCam.enabled = false;
    }

    public void ThirdPerson2()
    {
        thirdPersonCam.enabled = false;
        thirdPersonCam2.enabled = true;
        firstPersonCam.enabled = false;
    }

    public void FirstPerson()
    {
        thirdPersonCam.enabled = false;
        thirdPersonCam2.enabled = false;
        firstPersonCam.enabled = true;
    }
}