
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeRenderer : MonoBehaviour
{
    public void ChangeRenderers()
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


            //Debug.Log(object.name);
        }
    }

    public void AddRB()
    {
        DetachableBone[] bones = GetComponentsInChildren<DetachableBone>();

        for (int i = 0; i < bones.Length; i++)
        {
            if(bones[i].GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb =  bones[i].gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(ChangeRenderer))]
public class ChangeRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChangeRenderer script = (ChangeRenderer)target;

        if (GUILayout.Button("Change Renderers", GUILayout.Height(25)))
        {
            script.ChangeRenderers();
        }

        if (GUILayout.Button("Add RB", GUILayout.Height(25)))
        {
            script.AddRB();
        }
    }
}


#endif