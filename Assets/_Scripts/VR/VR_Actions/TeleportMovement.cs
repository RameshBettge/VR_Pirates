﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMovement : MonoBehaviour
{
    enum TeleportHand { Left, Right, Both }
    [SerializeField]
    TeleportHand teleportHand;

    [SerializeField]
    Renderer rightLine;

    [SerializeField]
    Renderer leftLine;

    public VRInputLookup VRInput;

    [SerializeField]
    float turnDegrees = 15f;

    [SerializeField]
    float turnTriggerAmount = 0.75f;

    [SerializeField]
    float turnInterval = 0.1f;

    [SerializeField]
    float lineLength = 100f;


    [SerializeField]
    Color lineColorAble;
    [SerializeField]
    Color lineColorUnable;

    [SerializeField]
    LayerMask teleportRayMask;

    [SerializeField]
    VRButton teleportDisplay;
    InputButton teleDisplayButtonL;
    InputButton teleDisplayButtonR;

    [SerializeField]
    VRButton teleport;
    InputButton teleportButtonL;
    InputButton teleportButtonR;

    [SerializeField]
    AxisPicker TurnAxis;
    InputAxis turnAxis;

    RaycastHit hit;
    bool canTeleport;

    bool turnedRight;
    bool turnedLeft;
    bool teleportedL;
    bool teleportedR;

    public float debugF;

    float handScale;

    float lastTurn;

    VRComponentFinder finder;

    private void Start()
    {
        finder = GetComponent<VRComponentFinder>();
        handScale = finder.RightHand.transform.lossyScale.x;
        VRInput = finder.lookup;

        turnAxis = TurnAxis.GetAxis(VRInput);

        Vector3 lineScale = rightLine.transform.localScale;
        lineScale.y = lineLength;
        rightLine.transform.localScale = lineScale;
        leftLine.transform.localScale = lineScale;

        rightLine.enabled = false;
        leftLine.enabled = false;
    }

    private void Update()
    {
        // Enable teleporting for the hand(s) which is specified by teleportHand-enum
        switch (teleportHand)
        {
            case TeleportHand.Left:
                teleportedL = Teleportation(true, VRInput, teleportedL);
                break;
            case TeleportHand.Right:
                teleportedR = Teleportation(false, VRInput, teleportedR);
                break;
            case TeleportHand.Both:
                teleportedL = Teleportation(true, VRInput, teleportedL);
                teleportedR = Teleportation(false, VRInput, teleportedR);
                break;
            default:
                break;
        }

        Turn();

    }

    private bool Teleportation(bool left, VRInputLookup l, bool teleported)
    {
        Renderer line = left ? leftLine : rightLine;

        InputButton teleDisplayButton = ButtonPicker.GetButton(l, teleportDisplay, left);
        InputButton teleportButton = ButtonPicker.GetButton(l, teleport, left);

        canTeleport = false;

        // TODO: don't set shader values if the renderer isn't active anyway
        if (Physics.Raycast(line.transform.position, line.transform.up * lineLength, out hit, teleportRayMask))
        {
            line.material.SetFloat("uMaxPos", hit.distance / lineLength / handScale);

            if (hit.collider.CompareTag("VRWalkable") && hit.normal.y > 0.2f)
            {
                line.material.SetFloatArray("uColor", lineColorAble.ToArray());
                canTeleport = true;
            }
        }

        if (!canTeleport)
        {
            line.material.SetFloat("uMaxPos", 100);
            line.material.SetFloatArray("uColor", lineColorUnable.ToArray());
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
            if (canTeleport && !teleported)
            {
                Vector3 targetPos = hit.point;
                Vector3 newHeadPos = Vector3.zero;
                newHeadPos.y = finder.Head.localPosition.y;
                finder.Head.localPosition = newHeadPos;

                transform.position = hit.point - finder.Head.localPosition;

                // TODO: check if near wall. set player away from it.

                teleported = true;
            }
        }
        else if (teleported)
        {
            teleported = false;
        }

        return teleported;
    }

    private void Turn()
    {
        float input = turnAxis.Value;

        bool turnTime = (Time.time - lastTurn) > turnInterval;

        if (turnedRight && !turnTime)
        {
            if (input < turnTriggerAmount)
            {
                turnedRight = false;
            }
        }
        else
        {
            if (input > turnTriggerAmount)
            {
                lastTurn = Time.time;
                turnedRight = true;
                transform.eulerAngles += Vector3.up * turnDegrees;
            }
        }

        if (turnedLeft && !turnTime)
        {
            if (input > -turnTriggerAmount)
            {
                turnedLeft = false;
            }
        }
        else
        {
            if (input < -turnTriggerAmount)
            {
                lastTurn = Time.time;
                turnedLeft = true;
                transform.eulerAngles -= Vector3.up * turnDegrees;
            }
        }

    }
}
