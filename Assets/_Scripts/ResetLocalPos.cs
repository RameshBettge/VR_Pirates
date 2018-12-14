using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLocalPos : MonoBehaviour
{

    private void LateUpdate()
    {
        transform.localPosition = Vector3.zero;
    }
}
