using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightDir : MonoBehaviour {

    [SerializeField]
    Material seaMat;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
        Vector3 fwd = transform.forward;
        seaMat.SetVector("_LightDir", new Vector4(fwd.x, fwd.y, fwd.z, 0f));
	}
}
