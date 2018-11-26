using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// TODO: Automatically call CopyFromLookup and controllers.write to list when switching vrInputLookup. Maybe check in update.
// TODO: Make the user able to hide the Buttons- and Axes-sections respectively.

[ExecuteInEditMode]
public class VRInputSetup : MonoBehaviour
{


    public enum Hand { Right, Left };

    public VRInputLookup vrInputLookup;

    [HideInInspector] public Hand Controller;
    [HideInInspector]
    public VRController currentController
    {
        get { return Controller == Hand.Left ? vrInputLookup.Left : vrInputLookup.Right; }
    }
    public VRController otherController
    {
        get { return Controller == Hand.Left ? vrInputLookup.Right : vrInputLookup.Left; }
    }

    [HideInInspector]
    public int lastButtonPressed = 0;
    [HideInInspector]
    public string lastAxisUsed = "none";
    [HideInInspector]
    public bool lastAxisNegative;

    [HideInInspector]
    public List<string> AxesUsedCurrent;
    [HideInInspector]
    public List<string> AxesUsedOther;

    [HideInInspector]
    public List<int> KeysUsedCurrent;
    [HideInInspector]
    public List<int> KeysUsedOther;


    // saved buttons
    [HideInInspector] public int button1 = -1;
    [HideInInspector] public int button1_Touch = -1;

    [HideInInspector] public int button2 = -1;
    [HideInInspector] public int button2_Touch = -1;

    [HideInInspector] public int index_Touch = -1;

    [HideInInspector] public int thumb_Touch = -1;
    [HideInInspector] public int thumb_Press = -1;

    // saved axes
    [HideInInspector] public int thumbX = -1;
    [HideInInspector] public bool thumbXInverted = false;

    [HideInInspector] public int thumbY = -1;
    [HideInInspector] public bool thumbYInverted = false;

    [HideInInspector] public int index = -1;
    [HideInInspector] public bool indexInverted = false;

    [HideInInspector] public int grab = -1;
    [HideInInspector] public bool grabInverted = false;


    [HideInInspector] public bool Button1Status;
    [HideInInspector] public bool Button1_TouchStatus;

    [HideInInspector] public bool Button2Status;
    [HideInInspector] public bool Button2_TouchStatus;

    [HideInInspector] public bool Index_TouchStatus;

    [HideInInspector] public bool Thumb_TouchStatus;
    [HideInInspector] public bool Thumb_PressStatus;

    [HideInInspector] public bool ThumbXStatus;
    [HideInInspector] public bool ThumbYStatus;

    [HideInInspector] public bool IndexStatus;
    [HideInInspector] public bool GrabStatus;


    private void SetStatuses()
    {
        // Buttons
        Button1_TouchStatus = ButtonIsPressed(button1_Touch) ? true : false;
        Button1Status = ButtonIsPressed(button1) ? true : false;

        Button2_TouchStatus = ButtonIsPressed(button2_Touch) ? true : false;
        Button2Status = ButtonIsPressed(button2) ? true : false;

        Index_TouchStatus = ButtonIsPressed(index_Touch) ? true : false;

        Thumb_TouchStatus = ButtonIsPressed(thumb_Touch) ? true : false;
        Thumb_PressStatus = ButtonIsPressed(thumb_Press) ? true : false;

        // Axes
        ThumbXStatus = AxisIsUsed(thumbX, thumbXInverted) ? true : false;
        ThumbYStatus = AxisIsUsed(thumbY, thumbYInverted) ? true : false;

        IndexStatus = AxisIsUsed(index, indexInverted) ? true : false;
        GrabStatus = AxisIsUsed(grab, grabInverted) ? true : false;
    }

    bool ButtonIsPressed(int num)
    {
        if (num < 0) { return false; }

        if (Input.GetKey(InputButton.JoystickFromInt(num)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool AxisIsUsed(int num, bool inverted)
    {
        float min = 0.5f;

        if (num < 0) { return false; }

        string name = InputAxis.FromInt(num);

        float input = Input.GetAxis(name);

        if (input > min && !inverted) { return true; }
        if (input < -min && inverted) { return true; }

        return false;
    }


    private void Awake()
    {
        CopyFromLookup();
        lastAxisUsed = "None";
    }

    public void CopyFromLookup()
    {
        KeysUsedCurrent = new List<int>();
        KeysUsedOther = new List<int>();
        AxesUsedCurrent = new List<string>();
        AxesUsedOther = new List<string>();

        currentController.WriteToSetup(this);

        currentController.WriteIntoList(this, KeysUsedCurrent, AxesUsedCurrent);
        otherController.WriteIntoList(this, KeysUsedOther, AxesUsedOther);
    }

    public void AddButton(int num, List<int> l)
    {
        if (num < 0) { return; }

        l.Add(num);
    }
    public void AddAxis(int num, bool inverted, List<string> l)
    {
        if (num < 0) { return; }
        string axisName = InputAxis.FromIntBool(num, inverted);

        l.Add(axisName);
    }


    // Uses duplicate code from KeyCodeToInt()
    public int AxisToInt(string axisName)
    {
        char[] chars = axisName.ToCharArray();
        char lastC = chars[chars.Length - 1];

        int result = (int)char.GetNumericValue(lastC);

        char secondLastC = chars[chars.Length - 2];
        if (System.Char.IsDigit(secondLastC))
        {
            result += ((int)char.GetNumericValue(secondLastC)) * 10;
        }

        return result;
    }

    public bool KeyCodeToInt(KeyCode kC, out int out_result, bool enableWarning = false)
    {
        string warning = "Key pressed is not a joystick button!";

        out_result = -1;
        char[] chars = kC.ToString().ToCharArray();

        // Check if it is a joystick button
        if (chars[0] != 'J' || chars[1] != 'o')
        {
            if (enableWarning)
            {
                Debug.LogWarning(warning);
            }

            return false;
        }

        // check if there is a number to be parsed.
        char lastC = chars[chars.Length - 1];
        if (!System.Char.IsDigit(lastC))
        {
            if (enableWarning)
            {
                Debug.LogWarning(warning);
            }
            return false;
        }

        out_result = (int)char.GetNumericValue(lastC);
        // check if the number at the end has 2 digits.
        char secondLastC = chars[chars.Length - 2];
        if (System.Char.IsDigit(secondLastC))
        {
            out_result += ((int)char.GetNumericValue(secondLastC)) * 10;
        }

        return true;
    }

    void Update()
    {
        SetStatuses();

        SetLastInput();
    }

    void SetLastInput()
    {
        for (int i = 1; i < 29; i++)
        {
            string axis = "Axis" + i;
            float v = Input.GetAxis(axis);

            if (v > 0.5f && v < 0.98f)
            {
                string name = InputAxis.FromIntBool(i, false);

                if (AxesUsedCurrent.Contains(name)) { return; }

                if (AxesUsedOther.Contains(name))
                {
                    Debug.LogWarning("Warning: " + name + " is already assigned to the other Controller. It cannot appear as 'last Axis used!'");
                    return;
                }

                lastAxisUsed = InputAxis.FromIntBool(i, false);
                lastAxisNegative = false;
            }
            if (v < -0.5f && v > -0.98f)
            {
                string name = InputAxis.FromIntBool(i, true);

                if (AxesUsedCurrent.Contains(name)) { return; }

                if (AxesUsedOther.Contains(name))
                {
                    Debug.LogWarning(name + " is already assigned to the other Controller. It cannot appear as 'last Axis used!'");
                    return;
                }

                lastAxisUsed = InputAxis.FromIntBool(i, true);
                lastAxisNegative = true;
            }
        }

        for (int i = 0; i < 20; i++) // Checks only the main joystick buttons
        {
            string name = "JoystickButton" + i;

            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), name);

            if (Input.GetKeyDown(key))
            {
                if (KeysUsedCurrent.Contains(i)) { return; }

                if (KeysUsedOther.Contains(i))
                {
                    Debug.LogWarning("The JoystickButton" + i + " is already assigned to the other Controller. It cannot appear as 'last Button pressed!'");
                    return;
                }

                lastButtonPressed = i;
            }
        }
    }

    public void DisplayManual()
    {
        string message =
            "     ------         General         ------     " +
            "\n" +
            "\n" +
            "First assign the VRInputLookup you want to fill.\n" +
            "To create a new one:\n" +
            "Right-click in Project-folder -> Create -> Lookups -> VRInput\n" +
            "\n" +
            "Start Play-Mode.\n" +
            "\n" +
            "Press a JoystickButton and it will be assigned to 'Last Button Pressed', unless it already has been assigned.\n" +
            "\n" +
            "Press 'Set' below a button, to assign the 'Last Button Pressed' to it.\n" +
            "\n" +
            "Axes work the exact same way.\n" +
            "\n" +
            "After assigning all Buttons and Axes on one Controller, click on apply and then on 'Switch Controller' to set up the other Controller.\n" +
            "\n" +
            "\n" +
            "             IMPORTANT WARNING:\n" +
            "If the 'Apply' button is not pressed before leaving play-mode, your changes will be lost!\n" +
            "\n" +
            "\n" +
            "     ------     Button specific     ------     " +
            "\n" +
            "\n" +
            "Always assign the 'Touch' buttons first! This way it is safe to assign 'Press' buttons without accidently using the 'touch' variant of the same button.\n" +
            "\n" +
            "\n" +
            "     ------      Axis specific      ------     " +
            "\n" +
            "\n" +
            "An axis will not be recognized if it is exactly 1 or -1. This is necessary to avoid assigning the 'near pressed' axis, which only returns integers.\n" +
            "\n" +
            "If an axis is a joystick always move it in positive direction (right/up).\n" +
            "This script recognizes if an axis is inverted that way." +
            "\n" +
            "\n" +
            "\n" +
            "     ------      Troubleshoot       ------     " +
            "\n" +
            "\n" +
            "Check the Debug Log.\n" +
            "\n" +
            "Input can only be recognized if:\n" +
            " - In play mode.\n" +
            " - Currently in game (click in game window)";


        EditorUtility.DisplayDialog("How to use the VRInputSetup", message, "Close");
    }

    public void DisplayAbout()
    {
        string message =
            "A VRInputLookup can be used to get input from VR Controllers very easily and comfortably.\n" +
            "\n" +
            "For each Headset your game should support, you can create a VRInputLookup. To quickly fill it, this script (VRInputLookupSetup.cs) can be used.";

        if(EditorUtility.DisplayDialog("About VRInputSetup", message, "Display Manual", "Close"))
        {
            DisplayManual();
        }
    }

    public string HandStatus()
    {
        return Controller == Hand.Left ? "Left" : "Right";
    }



    public void AskForSwitch()
    {
        int response = EditorUtility.DisplayDialogComplex("Switching Controller ...", "Please apply your changes before switching", "Apply Settings", "Discard Settings", "Cancel");

        if (response == 0)
        {
            ApplySettings(false);
            SwitchController();
        }
        else if (response == 1)
        {
            SwitchController();
        }
    }

    public void SwitchController()
    {
        if (Controller == Hand.Left) { Controller = Hand.Right; }
        else if (Controller == Hand.Right) { Controller = Hand.Left; }
        CopyFromLookup();

        Debug.LogWarning("Switched to " + HandStatus() + " controller.");
    }

    public void ApplySettings(bool remindAboutOther)
    {
        if (vrInputLookup == null)
        {
            Debug.LogError("No Lookup assigned!");
            return;
        }

        currentController.CopyFromSetup(this);
        vrInputLookup.UpdateLastApplied();

        string hand = HandStatus();
        string warning = hand + " controller's settings applied.";
        if (remindAboutOther) { warning += " Please remember to set up the other controller as well!"; }

        Debug.LogWarning(warning);
    }
}

