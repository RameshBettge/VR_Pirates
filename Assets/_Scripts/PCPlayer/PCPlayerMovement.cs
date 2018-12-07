using UnityEngine;

public class PCPlayerMovement : MonoBehaviour
{
    public Transform cam;
    private Vector3 cameraRot;

    public Transform GameObject;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotate();
        RotateHead();
        LockMouse();
        ReleaseMouse();
        CopyRotationFromCam();
    }

    void Rotate()
    {
        float yRot;
        yRot = Input.GetAxis("Mouse X");
        transform.rotation *= Quaternion.Euler(0f, yRot, 0f);
    }

    void RotateHead()
    {
        float xRot = Input.GetAxis("Mouse Y");
        xRot *= -1;
        Vector3 oldRot = cameraRot;

        cameraRot += new Vector3(xRot, 0f, 0f);
        if (cameraRot.x < -90 || cameraRot.x > 70f)
        {
            cameraRot = oldRot;
        }
        cam.localRotation = Quaternion.Euler(cameraRot.x, cam.rotation.y, cam.rotation.z);
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

    void CopyRotationFromCam()
    {
        GameObject.transform.rotation = cam.rotation;
    }
}