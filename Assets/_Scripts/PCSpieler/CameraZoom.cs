﻿using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    int zoom = 10;
    int normal = 60;
    float smooth = 5f;
    [HideInInspector]
    public bool isZoomed = false;
    bool hasCrossHair = false;
    bool hasScope = false;
    public GameObject crosshair;
    public GameObject scop;

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
            hasScope = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasCrossHair = false;
            hasScope = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isZoomed = !isZoomed;
            hasCrossHair = !hasCrossHair;
            hasScope = !hasScope;
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

        if (hasScope)
        {
            scop.SetActive(true);
        }
        else
        {
            scop.SetActive(false);
        }
    }
}