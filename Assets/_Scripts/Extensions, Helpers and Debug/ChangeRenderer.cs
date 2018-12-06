
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

        for (int i = skinnedRends.Length-1; i >= 0; i--)
        {
            GameObject gO = skinnedRends[i].gameObject;
            Mesh mesh = skinnedRends[i].sharedMesh;
            DestroyImmediate(skinnedRends[i]);

           gO.AddComponent<MeshRenderer>();
            MeshFilter filter =gO.AddComponent<MeshFilter>();
            filter.mesh = mesh;


            //Debug.Log(object.name);
        }
    }
}

[ExecuteInEditMode]
[CustomEditor(typeof(ChangeRenderer))]
public class ChangeRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Change", GUILayout.Height(25)))
        {
            ChangeRenderer script = (ChangeRenderer)target;
            script.ChangeRenderers();
        }
    }
}


#endif