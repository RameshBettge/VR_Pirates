using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHolder : MonoBehaviour
{
    Transform body;

    void Start()
    {
        body = transform.GetChild(0);
    }

    void Update()
    {
        Vector3 euler = new Vector3(0f, transform.eulerAngles.y, 0f);

        transform.eulerAngles = euler;
    }
}
