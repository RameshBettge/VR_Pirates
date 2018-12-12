using UnityEngine;

public class PCPlayerMovement : MonoBehaviour
{
    public Transform thirdPersonCam;
    public Transform thirdPersonCam2;
    public Transform firstPersonCam;
    public Transform helperTransform;
    public Transform cannon;
    public Transform player;

    public WeaponSwitching weaponScript;

    private Vector3 cameraRot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotate();
        Noding();
        RotateCannon();
        LockMouse();
        ReleaseMouse();
    }

    void Rotate()
    {
        float yRot;
        yRot = Input.GetAxis("Mouse X");
        transform.rotation *= Quaternion.Euler(0f, yRot, 0f);
    }

    void Noding()
    {
        if (!weaponScript.cannonSelected)
        {
            float xRot = Input.GetAxis("Mouse Y");
            xRot *= -1;
            Vector3 oldRot = cameraRot;

            cameraRot += new Vector3(xRot, 0f, 0f);
            if (cameraRot.x < -90 || cameraRot.x > 50f)
            {
                cameraRot = oldRot;
            }
            helperTransform.localRotation = Quaternion.Euler(cameraRot.x, 0f, 0f);
            thirdPersonCam.localRotation = Quaternion.Euler(cameraRot.x, thirdPersonCam.rotation.y, thirdPersonCam.rotation.z);
            thirdPersonCam2.localRotation = Quaternion.Euler(cameraRot.x, thirdPersonCam2.rotation.y, thirdPersonCam2.rotation.z);
            firstPersonCam.localRotation = Quaternion.Euler(cameraRot.x, firstPersonCam.rotation.y, firstPersonCam.rotation.z); 
        }
    }

    void RotateCannon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 newRotation = new Vector3(0, cannon.transform.eulerAngles.y, 0);
            player.transform.eulerAngles = newRotation;
        }
    }

    public void LockMouse()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ReleaseMouse()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}