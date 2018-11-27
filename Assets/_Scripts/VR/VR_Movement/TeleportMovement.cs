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
    ButtonPicker TeleportDisplayButton;
    InputButton teleDisplayButton;

    [SerializeField]
    ButtonPicker TeleportButton;
    InputButton teleportButton;

    [SerializeField]
    AxisPicker TurnAxis;
    InputAxis turnAxis;

    RaycastHit hit;
    bool canTeleport;

    bool turnedRight;
    bool turnedLeft;
    bool teleported;

    public float debugF;

    float handScale;

    float lastTurn;

    private void Awake()
    {
        teleDisplayButton = TeleportDisplayButton.GetButton(VRInput);
        teleportButton = TeleportButton.GetButton(VRInput);
        turnAxis = TurnAxis.GetAxis(VRInput);

        Vector3 lineScale = line.transform.localScale;
        lineScale.y = lineLength;
        line.transform.localScale = lineScale;

        handScale = rightHand.transform.lossyScale.x;
    }

    private void Update()
    {
        Teleportation();
        Turn();

    }

    private void Teleportation()
    {
        canTeleport = false;

        // TODO: don't set shader values if the renderer isn't active anyway
        if (Physics.Raycast(line.transform.position, line.transform.up * lineLength, out hit))
        {
            line.material.SetFloat("uMaxPos", hit.distance / lineLength / handScale);

            if (hit.collider.CompareTag("VRWalkable"))
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
                transform.position = hit.point;

                // TODO: check if near wall. set player away from it.

                teleported = true;
            }
        }
        else if (teleported)
        {
            teleported = false;
        }
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
