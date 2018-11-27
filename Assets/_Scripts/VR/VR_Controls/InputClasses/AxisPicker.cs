using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxisPicker
{
    [SerializeField]
    VRAxis axis;

    [SerializeField]
    bool leftHand = false;

    public InputAxis GetAxis(VRInputLookup l)
    {
        VRController c = leftHand ? l.Left : l.Right;

        switch (axis)
        {
            case VRAxis.ThumbX:
                return c.ThumbX;
            case VRAxis.ThumbY:
                return c.ThumbY;
            case VRAxis.Index:
                return c.Index;
            case VRAxis.Grab:
                return c.Grab;
            default:
                return null;
        }
    }

}

public enum VRAxis
{
    ThumbX, ThumbY, Index, Grab
}
