
using UnityEngine;
using System.Collections.Generic;
using System;

// TODO: Make sure the variables reflect the actual state even when set by VRInputSetup.cs!
[ExecuteInEditMode]
[System.Serializable]
public class VRController
{
    [Header("Buttons")]
    [SerializeField] int button1_Touch = -1;
    [SerializeField] int button2_Touch = -1;
    [Space(10)]

    [SerializeField] int index_Touch = -1;

    [SerializeField] int thumb_Touch = -1;
    [Space(10)]

    [SerializeField] int button1 = -1;
    [SerializeField] int button2 = -1;
    [SerializeField] int thumb_Press = -1;
    [Space(10)]


    [Header("Axes")]
    [SerializeField] int thumbX = -1;
    [SerializeField] bool thumbXInverted = false;
    [Space(10)]

    [SerializeField] int thumbY = -1;
    [SerializeField] bool thumbYInverted = false;
    [Space(10)]

    [SerializeField] int index = -1;
    [SerializeField] bool indexInverted = false;
    [Space(10)]

    //int rIndex_NearTouch = -1;

    [SerializeField] int grab = -1;
    [SerializeField] bool grabInverted = false;

    // ----

    [HideInInspector] [SerializeField] public InputButton Button1;
    [HideInInspector] [SerializeField] public InputButton Button1_Touch;
    [Space(10)]

    [HideInInspector] [SerializeField] public InputButton Button2;
    [HideInInspector] [SerializeField] public InputButton Button2_Touch;
    [Space(10)]

    [HideInInspector] public InputAxis ThumbX;
    [HideInInspector] public InputAxis ThumbY;
    [HideInInspector] [SerializeField] public InputAxisPair ThumbAxes;

    [HideInInspector] [SerializeField] public InputButton Thumb_Touch;
    [HideInInspector] [SerializeField] public InputButton Thumb_Press;
    [Space(10)]


    [HideInInspector] [SerializeField] public InputAxis Index;
    [HideInInspector] [SerializeField] public InputButton Index_Touch;
    [Space(10)]
    [HideInInspector] [SerializeField] public InputAxis Grab;

    //InputAxis rIndex_NearTouch;
    public void CopyFromSetup(VRInputSetup s)
    {
        button1 = s.button1;

        button1_Touch = s.button1_Touch;

        button2 = s.button2;
        button2_Touch = s.button2_Touch;

        index_Touch = s.index_Touch;

        thumb_Touch = s.thumb_Touch;
        thumb_Press = s.thumb_Press;


        thumbX = s.thumbX;
        thumbXInverted = s.thumbXInverted;

        thumbY = s.thumbY;
        thumbYInverted = s.thumbYInverted;

        index = s.index;
        indexInverted = s.indexInverted;

        grab = s.grab;
        grabInverted = s.grabInverted;



        Apply();
    }

    public void WriteToSetup(VRInputSetup s)
    {
        //Buttons
        s.button1 = button1;
        s.button1_Touch = button1_Touch;

        s.button2 = button2;
        s.button2_Touch = button2_Touch;

        s.index_Touch = index_Touch;

        s.thumb_Touch = thumb_Touch;
        s.thumb_Press = thumb_Press;

        //Axes
        s.thumbX = thumbX;
        s.thumbXInverted = thumbXInverted;

        s.thumbY = thumbY;
        s.thumbYInverted = thumbYInverted;

        s.index = index;
        s.indexInverted = indexInverted;

        s.grab = grab;
        s.grabInverted = grabInverted;
    }

    public void WriteIntoList(VRInputSetup s, List<int> bL, List<string> aL)
    {
        //UsedKeys
        s.AddButton(button1, bL);
        s.AddButton(button1_Touch, bL);
        s.AddButton(button2, bL);
        s.AddButton(button2_Touch, bL);
        s.AddButton(index_Touch, bL);
        s.AddButton(thumb_Touch, bL);
        s.AddButton(thumb_Press, bL);

        //UsedAxes
        s.AddAxis(thumbX, thumbXInverted, aL);
        s.AddAxis(thumbY, thumbYInverted, aL);
        s.AddAxis(index, indexInverted, aL);
        s.AddAxis(grab, grabInverted, aL);
    }


    public void Apply()
    {
        Button1 = new InputButton(button1);

        Button1_Touch = new InputButton(button1_Touch);

        Button2 = new InputButton(button2);
        Button2_Touch = new InputButton(button2_Touch);


        ThumbX = new InputAxis(thumbX, thumbXInverted);
        ThumbY = new InputAxis(thumbY, thumbYInverted);

        ThumbAxes = new InputAxisPair(ThumbX, ThumbY);

        Thumb_Touch = new InputButton(thumb_Touch);
        Thumb_Press = new InputButton(thumb_Press);


        Index = new InputAxis(index, indexInverted);
        //int rIndex_NearTouch;
        Index_Touch = new InputButton(index_Touch);
        Grab = new InputAxis(grab, grabInverted);
    }
}


