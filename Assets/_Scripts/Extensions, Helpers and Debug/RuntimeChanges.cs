


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RuntimeChanges : MonoBehaviour
{
    [SerializeField]
    Rigidbody rbToCopy;

    [Tooltip("Values to copy: maxVelocity, maxAngularVelocity, VelocityFactor, angularVelocityFactor, extraGravity")]
    [SerializeField]
    GrabbableObject grabToCopy;

    public void CopyRBSettings()
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i].gameObject == gameObject) { return; } // Don't set the rigidbody of the skeleton itself.

            bodies[i].drag = rbToCopy.drag;
            bodies[i].angularDrag = rbToCopy.angularDrag;
        }
    }

    public void CopyGrabbableSettings()
    {
        GrabbableObject[] grabbables = GetComponentsInChildren<GrabbableObject>();

        for (int i = 0; i < grabbables.Length; i++)
        {
            grabbables[i].maxVelocity = grabToCopy.maxVelocity;
            grabbables[i].maxAngularVelocity = grabToCopy.maxAngularVelocity;

            grabbables[i].velocityFactor = grabToCopy.velocityFactor;
            grabbables[i].angularVelocityFactor = grabToCopy.angularVelocityFactor;
            grabbables[i].extraGravity = grabToCopy.extraGravity;

#if UNITY_EDITOR
            EditorUtility.SetDirty(grabbables[i]);
#endif
        }
    }

    public void AddGrabbableObject()
    {
        DetachableBone[] bones = GetComponentsInChildren<DetachableBone>();

        for (int i = 0; i < bones.Length; i++)
        {
            GrabbableObject grabObj = bones[i].GetComponent<GrabbableObject>();

            if (grabObj == null)
            {
                bones[i].gameObject.AddComponent<GrabbableObject>();
            }
        }
    }

    public void ChangeRends()
    {
        SkinnedMeshRenderer[] skinnedRends = GetComponentsInChildren<SkinnedMeshRenderer>();
        print("total: " + skinnedRends.Length);

        for (int i = skinnedRends.Length - 1; i >= 0; i--)
        {
            GameObject gO = skinnedRends[i].gameObject;
            Mesh mesh = skinnedRends[i].sharedMesh;
            DestroyImmediate(skinnedRends[i]);

            gO.AddComponent<MeshRenderer>();
            MeshFilter filter = gO.AddComponent<MeshFilter>();
            filter.mesh = mesh;
        }
    }

    public void AddRB()
    {
        DetachableBone[] bones = GetComponentsInChildren<DetachableBone>();

        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i].GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = bones[i].gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }


}

#if UNITY_EDITOR
[ExecuteInEditMode]
[CustomEditor(typeof(RuntimeChanges))]
public class RuntimeChangesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RuntimeChanges script = (RuntimeChanges)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Copy RB Settings", GUILayout.Height(25)))
        {
            script.CopyRBSettings();
        }

        if (GUILayout.Button("Copy GrabbableObjects Settings", GUILayout.Height(25)))
        {
            script.CopyGrabbableSettings();
        }

        if (GUILayout.Button("Add Grabbable Object", GUILayout.Height(25)))
        {
            script.AddGrabbableObject();
        }

        if (GUILayout.Button("Change Renderers", GUILayout.Height(25)))
        {
            script.ChangeRends();
        }

        if (GUILayout.Button("Add RB", GUILayout.Height(25)))
        {
            script.AddRB();
        }
    }
}


#endif