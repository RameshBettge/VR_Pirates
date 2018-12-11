using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBouyancy : MonoBehaviour
{

    [SerializeField]
    SeaMovement sea;

    [SerializeField]
    float maxUpMovementPerSecond;

    [SerializeField]
    int maxTiltSamples = 10;

    int samples;

    [SerializeField]
    Transform front;

    [SerializeField]
    Transform back;

    [SerializeField]
    Transform left;

    [SerializeField]
    Transform right;

    float distanceToBack;

    Vector3 averageFwd;
    Vector3 averageRight;

    private void Start()
    {
        samples = 1;
        //distanceToBack = (transform.position - back.position).magnitude;
    }

    private void Update()
    {
        Vector3 fwdDir = GetTiltDir(back, front);

        Vector3 rightDir = GetTiltDir(left, right);


        if (samples == 1)
        //if (true)
        {
            averageFwd = fwdDir;
            averageRight = rightDir;
        }
        else
        {
            averageFwd /= samples;
            averageFwd *= (samples - 1);
            averageFwd += fwdDir / samples;


            averageRight /= samples;
            averageRight *= (samples - 1);
            averageRight += rightDir / samples;
        }

        Vector3 upDir = Vector3.Cross(averageFwd.normalized, averageRight.normalized);

        transform.rotation = Quaternion.LookRotation(averageFwd, upDir);

        //Vector3 newPos = GetSeaPosition(transform);
        transform.position = GetSeaPosition(transform);

        if (samples < maxTiltSamples)
        {
            samples++;
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
        float height = sea.GetHeight(t.position);
        Vector3 pos = t.position;
        pos.y = height;

        return pos;
    }
}