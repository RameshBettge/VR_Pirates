using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    int zoom = 10;
    int normal = 60;
    float smooth = 5f;
    [HideInInspector]
    public bool isZoomed = false;
    bool hasCrossHair = false;
    public GameObject crosshair;

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
            hasCrossHair = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasCrossHair = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isZoomed = !isZoomed;
            hasCrossHair = !hasCrossHair;
        }

        if (isZoomed)
        {
            cam.fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }

        if (hasCrossHair)
        {
            crosshair.SetActive(true);
        }
        else
        {
            crosshair.SetActive(false);
        }
    }
}