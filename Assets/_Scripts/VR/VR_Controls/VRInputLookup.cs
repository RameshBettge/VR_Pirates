using UnityEngine;
using UnityEditor;

// TODO: Make all variables private and create functions which return the input as bool or floats.

[CreateAssetMenu(fileName = "VRInputLookup", menuName = "Lookups/VRInput", order = 1)]
public class VRInputLookup : ScriptableObject
{
    public VRController Right;
    [Space(10)]
    public VRController Left;
    [Space(20)]

    [Tooltip("MM/DD/YY")]
    public string LastApplied = "WARNING: UNAPPLIED!";

    public void UpdateLastApplied()
    {
        LastApplied = System.DateTime.Now.ToShortDateString() + " - " + System.DateTime.Now.ToShortTimeString();
    }
}



[CustomEditor(typeof(VRInputLookup))]
public class VRControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VRInputLookup script = (VRInputLookup)target;


        if (GUILayout.Button("Apply Settings"))
        {
            EditorUtility.SetDirty(target);

            script.Right.Apply();
            script.Left.Apply();

            script.UpdateLastApplied();
        }
    }
}