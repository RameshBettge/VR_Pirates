using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] float moveSpeed = 0.1f;
    [SerializeField] float rotSpeed = 0.1f;

    public VRInputLookup VRInput;

    void Start()
    {

    }

    void Update()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector2 i = VRInput.Left.ThumbAxes.Vec2;
        Vector3 localMove = camera.TransformDirection(new Vector3(i.x, 0f, i.y));

        localMove.y = 0f;

        transform.position += localMove * moveSpeed * Time.deltaTime;
    }

    private void Turn()
    {
        float angle = VRInput.Right.ThumbAxes.X;

        transform.eulerAngles += Vector3.up * angle * rotSpeed * Time.deltaTime;
    }
}
