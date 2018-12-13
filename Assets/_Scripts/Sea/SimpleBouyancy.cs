﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBouyancy : MonoBehaviour
{


    //[HideInInspector]
    public SeaMovement sea;

    [SerializeField]
    bool adjustTilt = true;

    [SerializeField]
    int maxTiltSamples = 10;
    int tiltSamples;

    [SerializeField]
    int maxHeightSamples = 10;
    int heightSamples;

    public bool manualUpdate;


    [SerializeField]
    Transform front;

    [SerializeField]
    Transform back;

    [SerializeField]
    Transform left;

    [SerializeField]
    Transform right;

    float distanceToBack;

    [HideInInspector]
    public Vector3 averageFwd;
    [HideInInspector]
    public Vector3 averageRight;

    float averageHeight;


    private void Start()
    {
        tiltSamples = 1;
        heightSamples = 1;
        //distanceToBack = (transform.position - back.position).magnitude;
    }

    private void Update()
    {
        if (!manualUpdate)
        {
            UpdateBouyancy();
        }
    }

    public void UpdateBouyancy()
    {
        if (adjustTilt)
        {
            SetTilt();
        }
        SetBoatHeight();
    }

    private void SetBoatHeight()
    {
        Vector3 newPos = GetSeaPosition(transform);


        if (heightSamples == 1)
        {
            averageHeight = newPos.y;
        }
        else
        {
            averageHeight /= heightSamples;
            averageHeight *= (heightSamples - 1);
            averageHeight += newPos.y / heightSamples;
        }

        //float heightDifference = newPos.y - transform.position.y;

        //float sign = Mathf.Sign(heightDifference);

        //float absDifference = Mathf.Abs(heightDifference);
        //float maxMovement = maxUpMovementPerSecond * Time.deltaTime;
        //if (absDifference > maxMovement)
        //{
        //    absDifference = maxMovement;
        //}

        //newPos.y = transform.position.y + absDifference * sign;
        newPos.y = averageHeight;
        transform.position = newPos;

        if (heightSamples < maxHeightSamples)
        {
            heightSamples++;
        }
    }

    void SetTilt()
    {
        Vector3 fwdDir = GetTiltDir(back, front);

        Vector3 rightDir = GetTiltDir(left, right);


        if (tiltSamples == 1)
        //if (true)
        {
            averageFwd = fwdDir;
            averageRight = rightDir;
        }
        else
        {
            averageFwd /= tiltSamples;
            averageFwd *= (tiltSamples - 1);
            averageFwd += fwdDir / tiltSamples;


            averageRight /= tiltSamples;
            averageRight *= (tiltSamples - 1);
            averageRight += rightDir / tiltSamples;
        }

        Vector3 upDir = Vector3.Cross(averageFwd.normalized, averageRight.normalized);

        transform.rotation = Quaternion.LookRotation(averageFwd, upDir);

        if (tiltSamples < maxTiltSamples)
        {
            tiltSamples++;
        }
    }

    Vector3 GetTiltDir(Transform minus, Transform plus)
    {
        Vector3 pPos = GetSeaPosition(plus);
        Vector3 mPos = GetSeaPosition(minus);

        return (pPos - mPos).normalized;
    }

    Vector3 GetSeaPosition(Transform t)
    {
        if(sea == null)
        {
            Debug.LogError(name + " is missing a reference to sea!");
            return Vector3.zero;
        }

        float height = sea.GetHeight(t.position);
        Vector3 pos = t.position;
        pos.y = height;

        return pos;
    }
}