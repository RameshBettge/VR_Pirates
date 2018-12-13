using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    public Transform thirdPersonCam;

    float horizontalSpeed = 2f;
    Vector3 cameraRot;

    void Update()
    {
        //move camera in relation to cannon
        float xRot = Input.GetAxis("Mouse Y");
        xRot *= -1;
        Vector3 oldRot = cameraRot;

        cameraRot += new Vector3(xRot, 0f, 0f);
        if (cameraRot.x < -90 || cameraRot.x > 70f)
        {
            cameraRot = oldRot;
        }
        thirdPersonCam.localRotation = Quaternion.Euler(cameraRot.x, thirdPersonCam.rotation.y, thirdPersonCam.rotation.z);

        //move cannon on mast up and down
        float z = horizontalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(0, 0, -z);
    }
}