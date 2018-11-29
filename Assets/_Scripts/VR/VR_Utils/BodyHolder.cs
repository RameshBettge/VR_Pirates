using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHolder : MonoBehaviour
{
    Transform body;

    public Vector3 debug;

    void Start()
    {
        body = transform.GetChild(0);
    }

    void Update()
    {
        Vector3 euler = transform.eulerAngles;

        debug = euler;

        euler.x = 0f;
        euler.z = 0f;
        transform.eulerAngles = euler;
    }
}
