#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(VRInputSetup))]
public class VRInputSetupInspector : Editor
{
    VRController c
    {
        get
        {
            return script.currentController;
        }
    }

    GUIStyle setStyle = new GUIStyle();
    GUIStyle errorStyle = new GUIStyle();
    GUIStyle unassignedStyle = new GUIStyle();
    GUIStyle controllerStyle = new GUIStyle();

    GUIStyle header = new GUIStyle();


    int LastButton { get { return script.lastButtonPressed; } }

    VRInputSetup script;

    public override void OnInspectorGUI()
    {
        script = (VRInputSetup)base.target;

        errorStyle.fontSize = 12;
        float lightness = 0.35f;
        setStyle.normal.textColor = new Color(lightness, lightness, lightness);

        errorStyle.fontSize = 12;
        errorStyle.normal.textColor = Color.red;
        errorStyle.fontStyle = FontStyle.Bold;

        unassignedStyle.fontSize = 10;
        unassignedStyle.normal.textColor = Color.yellow;
        unassignedStyle.normal.textColor = new Color(lightness, lightness, lightness);
        unassignedStyle.fontStyle = FontStyle.Italic;

        header.fontSize = 16;
        header.fontStyle = FontStyle.Bold;

        controllerStyle.fontSize = 12;
        controllerStyle.normal.textColor = new Color(lightness, lightness, lightness);
        controllerStyle.fontStyle = FontStyle.Bold;

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((VRInputSetup)target), typeof(VRInputSetup), false);
        GUI.enabled = true;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("About this script", GUILayout.Height(25)))
        {
            script.DisplayAbout();
        }

        Color bColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.5f, 1, 0.5f, 1);
        if (GUILayout.Button("Display Manual", GUILayout.Height(25)))
        {
            script.DisplayManual();
        }
        GUI.backgroundColor = bColor;
        GUILayout.EndHorizontal();

        AddSpace(3);


        SerializedObject so = base.serializedObject;
        EditorGUILayout.PropertyField(so.FindProperty("vrInputLookup"), true);
        so.ApplyModifiedProperties();

        AddSpace(3);

        
        // Choose controller for setup
        if (GUILayout.Button("Switch Controller to Set Up", GUILayout.Height(25)))
        {

            script.AskForSwitch();
        }

        EditorGUILayout.LabelField("Current Controller: " + HandStatus(), controllerStyle);

        


        AddSpace(3);


        // ------------------------ Buttons
        EditorGUILayout.LabelField("Buttons", header);

        AddSpace(2);

        script.button1_Touch = DrawButtonInfo("Button1_Touch", script.button1_Touch, script.Button1_TouchStatus);
        script.button2_Touch = DrawButtonInfo("Button2_Touch", script.button2_Touch, script.Button2_TouchStatus);

        script.index_Touch = DrawButtonInfo("Index_Touch", script.index_Touch, script.Index_TouchStatus);
        script.thumb_Touch = DrawButtonInfo("Thumb_Touch", script.thumb_Touch, script.Thumb_TouchStatus);

        script.button1 = DrawButtonInfo("Button1", script.button1, script.Button1Status);
        script.button2 = DrawButtonInfo("Button2", script.button2, script.Button2Status);

        script.thumb_Press = DrawButtonInfo("Thumb_Press", script.thumb_Press, script.Thumb_PressStatus);

        EditorGUILayout.LabelField("Last Button Pressed: " + script.lastButtonPressed.ToString(), EditorStyles.boldLabel);

        AddSpace(6);


        //  ------------------------------- Axes

        EditorGUILayout.LabelField("Axes", header);

        AddSpace(2);

        AxisParameters info = DrawAxisInfo("ThumbX", script.thumbX, script.thumbXInverted, script.ThumbXStatus);
        script.thumbX = info.num;
        script.thumbXInverted = info.inverted;

        info = DrawAxisInfo("ThumbY", script.thumbY, script.thumbYInverted, script.ThumbYStatus);
        script.thumbY = info.num;
        script.thumbYInverted = info.inverted;

        info = DrawAxisInfo("Index", script.index, script.indexInverted, script.IndexStatus);
        script.index = info.num;
        script.indexInverted = info.inverted;

        info = DrawAxisInfo("Grab", script.grab, script.grabInverted, script.GrabStatus);
        script.grab = info.num;
        script.grabInverted = info.inverted;

        EditorGUILayout.LabelField("Last Axis used: " + script.lastAxisUsed, EditorStyles.boldLabel);

        AddSpace(3);


        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Apply Settings", GUILayout.Height(50)))
        {
            EditorUtility.SetDirty(target);
            script.ApplySettings(true);
        }
        GUI.backgroundColor = bColor;

        AddSpace(3);
    }

    // ----------------------------- Methods


    int DrawButtonInfo(string title, int currentKey, bool status)
    {
        int output = currentKey; // return input

        EditorGUILayout.LabelField(title, EditorStyles.largeLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set"))
        {
            if (ButtonIsUnused(script.lastButtonPressed))
            {
                RemoveKey(currentKey);
                output = script.lastButtonPressed;
                script.AddButton(output, script.KeysUsedCurrent);
            }
        }
        if (GUILayout.Button("Reset"))
        {
            RemoveKey(currentKey);
            output = -1;
        }
        GUILayout.EndHorizontal();
        //EditorGUI.BeginDisabledGroup(true);
        if (status)
        {
            JoystickStatus(currentKey, true);
        }
        else
        {
            JoystickStatus(currentKey, false);
        }
        //EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        return output;
    }

    AxisParameters DrawAxisInfo(string title, int axisNum, bool inverted, bool status)
    {
        AxisParameters output = new AxisParameters(axisNum, inverted);


        EditorGUILayout.LabelField(title, EditorStyles.largeLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set"))
        {

            if (LastAxisIsUnused(axisNum, inverted))
            {
                RemoveAxis(axisNum, inverted);
                output.num = script.AxisToInt(script.lastAxisUsed);
                output.inverted = script.lastAxisNegative;
                script.AddAxis(output.num, output.inverted, script.AxesUsedCurrent);
            }
        }
        if (GUILayout.Button("Reset"))
        {
            RemoveAxis(axisNum, inverted);
            output.num = -1;
        }
        GUILayout.EndHorizontal();
        if (status)
        {
            AxisStatus(axisNum, inverted, true);
        }
        else
        {
            AxisStatus(axisNum, inverted, false);
        }

        EditorGUILayout.Space();

        return output;
    }


    string HandStatus()
    {
        if (script == null) { return "script null"; }
        return script.Controller == VRInputSetup.Hand.Left ? "Left" : "Right";
    }


    bool LastAxisIsUnused(int num, bool inverted)
    {
        string lastAxisUsed = script.lastAxisUsed;

        bool unused = true;

        bool originalIsInverted;

        string[] parts = lastAxisUsed.Split();

        string invertedName;
        string normalName;

        string oppositeName;

        if (parts.Length > 1) // if there is '(inverted)' before the axis name
        {
            invertedName = lastAxisUsed;
            normalName = parts[1];

            oppositeName = normalName;

            originalIsInverted = true;
        }
        else
        {
            normalName = lastAxisUsed;
            invertedName = "(inverted) " + lastAxisUsed;

            oppositeName = invertedName;

            originalIsInverted = false;
        }

        if (num > -1)
        {
            string current = InputAxis.FromIntBool(num, inverted);
            if (current == oppositeName) { return true; } // assigning the same axis but (un)inverted is possible.
        }

        // TODO: Avoid having 12 if statements for creating specific error messages.

        // Check current controller
        if (script.AxesUsedCurrent.Contains(normalName))
        {
            if (originalIsInverted)
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to current Controller in inverted variant.");
            }
            else
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to current Controller.");
            }
        }
        else if (script.AxesUsedCurrent.Contains(invertedName))
        {
            if (originalIsInverted)
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to current Controller.");
            }
            else
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to current Controller in un-inverted variant.");
            }
        }

        // Check other controller
        if (script.AxesUsedOther.Contains(normalName))
        {
            if (originalIsInverted)
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to other Controller in inverted variant.");
            }
            else
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to other Controller.");
            }
        }
        else if (script.AxesUsedOther.Contains(invertedName))
        {
            if (originalIsInverted)
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to other Controller.");
            }
            else
            {
                unused = false;
                Debug.LogError(lastAxisUsed + " is already assigned to other Controller in un-inverted variant.");
            }
        }

        return unused;
    }

    bool ButtonIsUnused(int keyNum)
    {
        if (script.KeysUsedCurrent.Contains(keyNum))
        {
            Debug.LogError("JoystickButton" + keyNum + " is already used by this Controller!");
            return false;
        }
        if (script.KeysUsedOther.Contains(keyNum))
        {
            Debug.LogError("JoystickButton" + keyNum + " is already used by the other Controller!");
            return false;
        }

        return true;
    }


    void JoystickStatus(int num, bool error)
    {
        if (num == -1)
        {
            EditorGUILayout.LabelField("Unassigned!", unassignedStyle);
        }
        else
        {
            if (error)
            {
                EditorGUILayout.LabelField("JoystickButton" + num, errorStyle);
            }
            else
            {
                EditorGUILayout.LabelField("JoystickButton" + num, setStyle);
            }
        }
    }

    void AxisStatus(int num, bool inverted, bool error)
    {
        if (num == -1)
        {
            EditorGUILayout.LabelField("Unassigned!", unassignedStyle);
        }
        else
        {
            if (error)
            {
                EditorGUILayout.LabelField(InputAxis.FromIntBool(num, inverted), errorStyle);
            }
            else
            {
                EditorGUILayout.LabelField(InputAxis.FromIntBool(num, inverted), setStyle);
            }
        }
    }

    void RemoveKey(int num)
    {
        if (script.KeysUsedCurrent.Contains(num))
        {
            script.KeysUsedCurrent.Remove(num);
        }
        else if (script.KeysUsedOther.Contains(num))
        {
            script.KeysUsedOther.Remove(num);
        }
    }

    void RemoveAxis(int num, bool inverted)
    {
        if (num < 0) { return; }
        string name = InputAxis.FromIntBool(num, inverted);

        if (script.AxesUsedCurrent.Contains(name))
        {
            script.AxesUsedCurrent.Remove(name);
        }
        else if (script.AxesUsedOther.Contains(name))
        {
            script.AxesUsedOther.Remove(name);
        }
    }

    void AddSpace(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            EditorGUILayout.Space();
        }
    }
}
#endif
