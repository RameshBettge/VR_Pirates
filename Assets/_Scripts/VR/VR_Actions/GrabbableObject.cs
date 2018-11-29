using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: (optional) disable collider when grabbed until directly after it is released.
// TODO: put inspector in own file

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour
{
    [SerializeField]
    bool disabledUntilGrabbed = true;

    [SerializeField]
    float maxVelocity = 5f;
    [SerializeField]
    float velocityFactor = 1.5f;
    [SerializeField]
    float angularVelocityFactor = 0.5f;
    [SerializeField]
    float maxAngularVelocity = 10f;


    [SerializeField]
    float extraGravity = 40f;

    
    public bool setRotOnGrab = false;
    [HideInInspector]
    public Vector3 defaultRot;


    [HideInInspector]
    public bool isGrabbed;

    Vector3 grabEuler;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = disabledUntilGrabbed;
        rb.useGravity = false;
        rb.maxAngularVelocity = maxAngularVelocity;


        //rb.maxAngularVelocity = maxAngularVelocity;

        int grabLayer = LayerMask.NameToLayer("Grabbable");
        if (gameObject.layer != grabLayer)
        {
            gameObject.layer = grabLayer;
            Debug.LogError(gameObject.name + "'s layer was changed to 'grabbable' - otherwise it couldn't be grabbed.");
        }
    }

    void Update()
    {
        if (rb.isKinematic) { return; }

        AddGravity();
    }

    private void AddGravity()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);
    }

    public void OnGrab()
    {
        if (setRotOnGrab)
        {
            transform.localEulerAngles = defaultRot;
        }

        rb.isKinematic = true;
        isGrabbed = true;
    }
    public void OnRelease(GrabData data, Transform rootObject, float deltaTime)
    {
        rb.isKinematic = false;
        isGrabbed = false;

        Vector3 averageVelocity = data.AverageMovement;

        Vector3 velocity = rootObject.TransformDirection(averageVelocity);

        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        rb.velocity = velocity * velocityFactor;


        Vector3 angularVelocity = data.AverageRotation * angularVelocityFactor;
        angularVelocity = Vector3.ClampMagnitude(angularVelocity, maxAngularVelocity);

        //angularVelocity = rootObject.rotation * angularVelocity;
        Vector3 localAngularVelocity = rootObject.TransformDirection(angularVelocity);
        rb.angularVelocity = localAngularVelocity;

        //Debug.Log(angularVelocity.magnitude);
    }
}


[ExecuteInEditMode]
[CanEditMultipleObjects]
[CustomEditor(typeof(GrabbableObject))]
public class GrabbableObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GrabbableObject script = (GrabbableObject)target;

        if (script.setRotOnGrab)
        {
            Vector3 newRot = EditorGUILayout.Vector3Field("Default Rot",script.defaultRot);
            if(newRot != script.defaultRot)
            {
                script.defaultRot = newRot;
                EditorUtility.SetDirty(target);
            }
        }
    }

}
