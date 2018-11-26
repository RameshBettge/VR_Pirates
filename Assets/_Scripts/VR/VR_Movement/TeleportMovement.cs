using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMovement : MonoBehaviour
{


    [SerializeField]
    Renderer line;

    // TODO: hand is just used for getting it's scale. Implement a method that doesn't need a serialized field.
    [SerializeField]
    Transform rightHand;

    [SerializeField]
    VRInputLookup VRInput;

    [SerializeField]
    float turnDegrees = 15f;

    [SerializeField]
    float turnArea = 0.75f;

    [SerializeField]
    float teleportationOffset;

    [SerializeField]
    float lineLength = 100f;

    [SerializeField]
    Color lineColorAble;
    [SerializeField]
    Color lineColorUnable;

    [SerializeField]
    ButtonPicker TeleportDisplayButton;
    InputButton teleDisplayButton;

    [SerializeField]
    ButtonPicker TeleportButton;
    InputButton teleportButton;

    RaycastHit hit;
    bool canTeleport;

    bool turnedRight;
    bool turnedLeft;
    bool teleported;

    public float debugF;

    float handScale;

    private void Awake()
    {
        teleDisplayButton = TeleportDisplayButton.GetButton(VRInput);
        teleportButton = TeleportButton.GetButton(VRInput);

        Vector3 lineScale = line.transform.localScale;
        lineScale.y = lineLength;
        line.transform.localScale = lineScale;

        //line.material.SetFloat("uMaxPos", 100);
        //line.material.SetFloatArray("uColor", lineColorUnable.ToArray());
    }

    private void Update()
    {
        Teleportation();
        Turn();

    }

    private void Teleportation()
    {

        // TODO: don't set shader values if the renderer isn't active anyway
        if (Physics.Raycast(line.transform.position, line.transform.up * lineLength, out hit))
        {
            line.material.SetFloat("uMaxPos", hit.distance / lineLength / rightHand.transform.lossyScale.x);
            //line.material.SetFloatArray("uColor", lineColorAble.ToArray());

            //line color = blue
            canTeleport = true;
        }
        else
        {
            line.material.SetFloat("uMaxPos", 100);
            //line.material.SetFloatArray("uColor", lineColorUnable.ToArray());


            // line color = red
            canTeleport = false;
        }

        if (teleDisplayButton.IsPressed)
        {
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
            canTeleport = false;
        }

        if (teleportButton.IsPressed)
        {


            if (!teleported)
            {

                Debug.DrawRay(rightHand.position, rightHand.forward * 100f, Color.cyan);


                if (canTeleport)
                {
                    Vector3 dir = hit.point - transform.position;
                    dir.y = 0f;
                    dir = dir.normalized * teleportationOffset;

                    Vector3 target = hit.point + dir;
                    target.y = transform.position.y;

                    transform.position = target;

                    teleported = true;
                }
            }
        }
        else if (teleported)
        {
            teleported = false;
        }
    }

    private void Turn()
    {
        float input = VRInput.Right.ThumbAxes.X;

        if (turnedRight)
        {
            if (input < turnArea)
            {
                turnedRight = false;
            }
        }
        else
        {
            if (input > turnArea)
            {
                turnedRight = true;
                transform.eulerAngles += Vector3.up * turnDegrees;
            }
        }

        if (turnedLeft)
        {
            if (input > -turnArea)
            {
                turnedLeft = false;
            }
        }
        else
        {
            if (input < -turnArea)
            {
                turnedLeft = true;
                transform.eulerAngles -= Vector3.up * turnDegrees;
            }
        }
    }
}
