using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyOwner : MonoBehaviour {


    [SerializeField]
    GameObject bodyPrefab;

    [SerializeField]
    Vector3 offset;

    Transform body;

	void Awake () {
        body = Instantiate(bodyPrefab, transform).transform;
        body.position = transform.position + offset;
        body.rotation = Quaternion.identity;
	}
	
	void Update () {
        Vector3 localOffset = transform.forward;

        localOffset.y = 0f;
        localOffset = localOffset.normalized * offset.z;
        localOffset.y = offset.y;

        body.rotation = Quaternion.identity;
        body.position = transform.position + localOffset;
        body.rotation = Quaternion.identity;

        body.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
