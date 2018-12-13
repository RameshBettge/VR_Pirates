using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: (optional) disable collider when grabbed until directly after it is released.
// TODO: put inspector in own file
// TODO: sometimes applying angular velocity on thrown objects isn't working properly, fix it. It seems like it only occurs around the forward-axis.

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour
{
    [SerializeField]
    bool disabledUntilGrabbed = false;

    [SerializeField]
    public float maxVelocity = 5f;
    [SerializeField]
    public float velocityFactor = 1.5f;
    [SerializeField]
    public float angularVelocityFactor = 0.5f;
    [SerializeField]
    public float maxAngularVelocity = 10f;


    [SerializeField]
    public float extraGravity = 40f;


    public bool setRotationOnGrab = false;
    public bool setPositionOnGrab = false;
    [HideInInspector]
    public Vector3 defaultRot;
    [HideInInspector]
    public Vector3 defaultPos;


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

        isGrabbed = false;

        //rb.maxAngularVelocity = maxAngularVelocity;

        int grabLayer = LayerMask.NameToLayer("Grabbable");
        if (gameObject.layer != grabLayer && GetComponent<DetachableBone>() == null)
        {
            //gameObject.layer = grabLayer;
            //Debug.LogError(gameObject.name + "'s layer was changed to 'grabbable' - otherwise it couldn't be grabbed.");
            Debug.LogWarning(gameObject.name + "'s layer is not set to 'grabbable' - it cannot be grabbed.");
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

    public void OnGrab(GrabData data)
    {


        if (setRotationOnGrab)
        {
            transform.localEulerAngles = defaultRot;
        }
        if (setPositionOnGrab)
        {
            Vector3 newPos = defaultPos;

            if (data.isLeft)
            {
                newPos.x *= -1f;
            }

            transform.localPosition = newPos;
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

#if UNITY_EDITOR
[ExecuteInEditMode]
[CanEditMultipleObjects]
[CustomEditor(typeof(GrabbableObject))]
public class GrabbableObjectInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GrabbableObject script = (GrabbableObject)target;

        if (script.setRotationOnGrab)
        {
            Vector3 newRot = EditorGUILayout.Vector3Field("Default Rot", script.defaultRot);
            if (newRot != script.defaultRot)
            {
                script.defaultRot = newRot;
                EditorUtility.SetDirty(target);
            }
        }
        if (script.setPositionOnGrab)
        {

            Vector3 newPos = EditorGUILayout.Vector3Field("Default Pos", script.defaultPos);
            if (newPos != script.defaultPos)
            {
                script.defaultPos = newPos;
                EditorUtility.SetDirty(target);
            }
        }
    }

}
#endif