using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBouyancy : MonoBehaviour {

    [SerializeField]
    SeaMovement sea;

    [SerializeField]
    Transform front;

    [SerializeField]
    Transform back;

    [SerializeField]
    Transform left;

    [SerializeField]
    Transform right;

    float distanceToBack;

    private void Start()
    {
        //distanceToBack = (transform.position - back.position).magnitude;
    }

    private void Update()
    {
        Vector3 fwdDir = GetTiltDir(back, front);

        Vector3 rightDir = GetTiltDir(left, right);

        Vector3 upDir = Vector3.Cross(fwdDir, rightDir);

        transform.rotation = Quaternion.LookRotation(fwdDir, upDir);

        //Vector3 boatPos = transform.position;
        //boatPos.y = sea.GetHeight(transform.position);
        transform.position = GetSeaPosition(transform);
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