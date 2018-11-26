using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonPicker
{
    [SerializeField]
    bool leftHand = false;
    [SerializeField]
    VRButton button;

    public InputButton GetButton(VRInputLookup l)
    {
        VRController c = leftHand ? l.Left : l.Right;

        return c.ButtonByEnum(button);
    }
}
