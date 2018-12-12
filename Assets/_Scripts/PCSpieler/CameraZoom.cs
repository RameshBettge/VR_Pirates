using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    int zoom = 10;
    int normal = 60;
    float smooth = 5f;
    [HideInInspector]
    public bool isZoomed = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            isZoomed = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isZoomed = !isZoomed;
        }

        if (isZoomed)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }
    }
}