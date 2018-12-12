using UnityEngine;

public class CameraSwitching : MonoBehaviour
{
    public Camera thirdPersonCam;
    public Camera firstPersonCam;

    public void Start()
    {
        thirdPersonCam.enabled = true;
        firstPersonCam.enabled = false;
    }

    public void FirstPerson()
    {
        thirdPersonCam.enabled = false;
        firstPersonCam.enabled = true;
    }

    public void ThirdPerson()
    {
        thirdPersonCam.enabled = true;
        firstPersonCam.enabled = false;
    }
}