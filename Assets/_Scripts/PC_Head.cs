using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Head : MonoBehaviour
{
    [SerializeField]
    Transform yRotator;

    float headTiltSpeed = 12f;

    float currentXRotation;
    //float currentYRotation;
    float convertedRotation;

    private void Awake()
    {

        Vector3 locEuler = transform.localEulerAngles;
            locEuler.x = 0f;
        transform.localEulerAngles = locEuler;

        currentXRotation = 0f;
        //currentYRotation = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float maxUpTilt = 65f;
        float maxDownTilt = 40f;

        //float maxYTilt = 75;



        float yRotation = Input.GetAxis("Mouse X");
        yRotator.Rotate(Vector3.up, yRotation * headTiltSpeed * Time.deltaTime);

        //float clampedZ = Mathf.Clamp(yRotator.eulerAngles.y, -maxYTilt, maxYTilt);
        //yRotator.localEulerAngles = Vector3.up * clampedY;


        float xRotation = -Input.GetAxis("Mouse Y");

        currentXRotation += xRotation * headTiltSpeed * Time.deltaTime;
        currentXRotation = Mathf.Clamp(currentXRotation, -maxUpTilt, maxDownTilt);

        convertedRotation = currentXRotation;
        if (convertedRotation < 0f)
        {
            convertedRotation += 360f;
        }

        transform.localEulerAngles = Vector3.right * convertedRotation;
    }
}
