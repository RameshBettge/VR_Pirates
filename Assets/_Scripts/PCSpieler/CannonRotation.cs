using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    public Transform thirdPersonCam;

    float horizontalSpeed = 2f;
    Vector3 cameraRot;

    public float debug;

    void Update()
    {
        //move camera in relation to cannon
        float xRot = Input.GetAxis("Mouse Y");
        xRot *= -1;
        Vector3 oldRot = cameraRot;

        cameraRot += new Vector3(xRot, 0f, 0f);
        if (cameraRot.x < -5 || cameraRot.x > 20f)
        {
            cameraRot = oldRot;
        }
        thirdPersonCam.localRotation = Quaternion.Euler(cameraRot.x, thirdPersonCam.rotation.y, thirdPersonCam.rotation.z);

        //move cannon on mast up and down
        float zAdd = -horizontalSpeed * Input.GetAxis("Mouse Y");

        float maxUpTilt = 60f;
        float maxDownTilt = 45f;

        float z = transform.eulerAngles.z + zAdd;

        if(z > 180f)
        {
            z -= 360f;
        }

        debug = z;

        z = Mathf.Clamp(z, -maxUpTilt, maxDownTilt);

        Vector3 cannonEuler = transform.eulerAngles;
        cannonEuler.z = z;
        transform.eulerAngles = cannonEuler;
        //transform.Rotate(0, 0, -z);
    }
}