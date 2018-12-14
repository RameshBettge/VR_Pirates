using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHolder : MonoBehaviour
{
    void Update()
    {
        Vector3 euler = new Vector3(0f, transform.eulerAngles.y, 0f);

        transform.eulerAngles = euler;
    }
}
