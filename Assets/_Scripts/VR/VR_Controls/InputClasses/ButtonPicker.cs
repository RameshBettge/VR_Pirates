using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonPicker
{
    [SerializeField]
    VRButton button;
    [SerializeField]
    bool leftHand = false;

    public InputButton GetButton(VRInputLookup l)
    {
        VRController c = leftHand ? l.Left : l.Right;

        switch (button)
        {
            case VRButton.Button1:
                return c.Button1;

            case VRButton.Button1_Touch:
                return c.Button1_Touch;
            case VRButton.Button2:
                return c.Button2;
            case VRButton.Button2_Touch:
                return c.Button2_Touch;
            case VRButton.Thumb_Touch:
                return c.Thumb_Touch;
            case VRButton.Thumb_Press:
                return c.Thumb_Press;
            case VRButton.Index_Touch:
                return c.Index_Touch;
        }

        return null;
    }

    public static InputButton GetButton(VRInputLookup l, VRButton button, bool left)
    {
        VRController c = left ? l.Left : l.Right;

        switch (button)
        {
            case VRButton.Button1:
                return c.Button1;

            case VRButton.Button1_Touch:
                return c.Button1_Touch;
            case VRButton.Button2:
                return c.Button2;
            case VRButton.Button2_Touch:
                return c.Button2_Touch;
            case VRButton.Thumb_Touch:
                return c.Thumb_Touch;
            case VRButton.Thumb_Press:
                return c.Thumb_Press;
            case VRButton.Index_Touch:
                return c.Index_Touch;
        }

        return null;
    }
}

public enum VRButton
{
    Button1, Button1_Touch, Button2, Button2_Touch, Thumb_Touch, Thumb_Press, Index_Touch
}
