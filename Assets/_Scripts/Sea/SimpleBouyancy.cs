using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBouyancy : MonoBehaviour
{
    [SerializeField]
    bool useHighestPos;

    //[HideInInspector]
    public SeaMovement sea;

    [SerializeField]
    bool adjustTilt = true;

    [SerializeField]
    int maxTiltSamples = 10;
    int tiltSamples;

    [HideInInspector]
    public int maxHeightSamples = 10;

    public int heightSamples;

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

    [HideInInspector]
    public float averageHeight;

    bool departing = false;
    float departHeight;
    float departStart;
    float departEnd;

    //[HideInInspector]
    //public bool sinkingIsLimited = false;
    //float sinkLimit;



    private void Start()
    {
        tiltSamples = 1;
        heightSamples = 1;
        //distanceToBack = (transform.position - back.position).magnitude;
    }

    private void Update()
    {
        String debug = name;

        if (!manualUpdate)
        {
            UpdateBouyancy();
        }

        if (departing)
        {
            Depart();
        }
    }

    //public void SetHeightLimit(float limit)
    //{
    //    sinkingIsLimited = true;
    //    sinkLimit = limit;
    //}

    private void Depart()
    {
        float end = departEnd - departStart;
        float current = Time.time - departStart;

        float percentage = current / end;

        if (percentage >= 1f)
        {
            departing = false;
            return;
        }

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(departHeight, pos.y, percentage);

        transform.position = pos;
    }

    public void OnDepart(float departDuration)
    {
        departHeight = transform.position.y;
        departing = true;
        departStart = Time.time;
        departEnd = departStart + departDuration;
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

        if (useHighestPos)
        {
            float highest = newPos.y;

            highest = TestIfHigher(front, highest);
            highest = TestIfHigher(back, highest);
            highest = TestIfHigher(left, highest);
            highest = TestIfHigher(right, highest);

            newPos.y = highest;
        }

        //float seaHeight = newPos.y;

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

        //if (sinkingIsLimited)
        //{
        //    if (averageHeight < seaHeight + sinkLimit)
        //    {
        //        averageHeight = seaHeight + sinkLimit;
        //    }
        //}

        newPos.y = averageHeight;
        transform.position = newPos;


        if (heightSamples < maxHeightSamples)
        {
            heightSamples++;
        }
    }

    float TestIfHigher(Transform boyancyHelper, float highest)
    {
        float seaPos = GetSeaPosition(boyancyHelper).y;
        return Mathf.Max(seaPos, highest);
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
        if (sea == null)
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