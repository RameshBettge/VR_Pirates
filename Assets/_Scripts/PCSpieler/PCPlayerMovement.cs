using UnityEngine;

public class PCPlayerMovement : MonoBehaviour
{
    [SerializeField]
    CameraZoom zoom;

    [SerializeField]
    float zoomSensitivity = 0.3f;

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
        RotateToCannon();
    }

    //rotate left/right
    void Rotate()
    {
        float yRot;
        yRot = Input.GetAxis("Mouse X");


        if (zoom.isZoomed)
        {
            yRot *= zoomSensitivity;
        }

        transform.rotation *= Quaternion.Euler(0f, yRot, 0f);
    }

    //rotate up/down, only if not cannon selected
    void Noding()
    {
        if (!weaponScript.cannonSelected)
        {
            float xRot = Input.GetAxis("Mouse Y");
            xRot *= -1;
            if (zoom.isZoomed)
            {
                xRot *= zoomSensitivity;
            }
            Vector3 oldRot = cameraRot;

            cameraRot += new Vector3(xRot, 0f, 0f);
            if (cameraRot.x < -60 || cameraRot.x > 45f) // TODO: Change these values depending on which weapon is equipped
            {
                cameraRot = oldRot;
            }
            helperTransform.localRotation = Quaternion.Euler(cameraRot.x, 0f, 0f);
            thirdPersonCam.localRotation = Quaternion.Euler(cameraRot.x, thirdPersonCam.rotation.y, thirdPersonCam.rotation.z);
            thirdPersonCam2.localRotation = Quaternion.Euler(cameraRot.x, thirdPersonCam2.rotation.y, thirdPersonCam2.rotation.z);
            firstPersonCam.localRotation = Quaternion.Euler(cameraRot.x, firstPersonCam.rotation.y, firstPersonCam.rotation.z); 
        }
    }

    void RotateToCannon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 newRotation = new Vector3(0, cannon.transform.eulerAngles.y, 0);
            player.transform.eulerAngles = newRotation;
        }
    }
}