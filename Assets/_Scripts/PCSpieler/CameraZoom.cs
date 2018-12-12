using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    int zoom = 10;
    int normal = 60;
    float smooth = 5f;
    [HideInInspector]
    public bool isZoomed = false;

    Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isZoomed = false;
            cam.fieldOfView = normal;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isZoomed = !isZoomed;
        }

        if (isZoomed)
        {
            cam.fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }
    }
}