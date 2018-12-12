using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatNavigation : MonoBehaviour
{
    [SerializeField]
    Docker docker;

    bool docking;

    [SerializeField]
    float speed = 3f;

    [SerializeField]
    float driftSpeed = 1f;

    [SerializeField]
    float dockingDistance = 1f;
    float sqrDockingDistance;

    [SerializeField]
    float dockingTime = 1f;

    [SerializeField]
    float sinkTime = 10f;
    [SerializeField]
    float sinkDepth = 10f;

    [SerializeField]
    AnimationCurve sinkCurve;

    SkeletonBoat skeletonBoat;

    float timer;

    Vector3 directionToDocker;

    BoatState state = BoatState.Moving;

    SimpleBouyancy bouyancy;



    void Awake()
    {
        // TODO: Find the nearest docker to steer towards instead of having a serializedField
        directionToDocker = (docker.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToDocker, Vector3.up);

        bouyancy = GetComponent<SimpleBouyancy>();
        bouyancy.manualUpdate = true;

        skeletonBoat = GetComponent<SkeletonBoat>();
    }


    void Update()
    {
        bouyancy.UpdateBouyancy();

        switch (state)
        {
            case BoatState.Moving:
                Move(directionToDocker, speed);
                CheckDistance();
                break;

            case BoatState.Docking:
                Dock();
                break;

            case BoatState.Boarding:
                Boarding();
                break;

            case BoatState.Sinking:
                Sink();
                break;

            default:
                break;
        }
    }

    void Boarding()
    {
        bool finished = skeletonBoat.Board(docker);
        if (finished)
        {
            state = BoatState.Sinking;
        }
    }

    private void Sink()
    {
        float speedModifier = 1f;

        timer += Time.deltaTime;

        if (timer < 1f)
        {
            speedModifier = timer;
        }

        float percentage = timer / sinkTime;

        if (percentage >= 1f)
        {
            Destroy(this);
            Destroy(gameObject);
            return;
        }
        percentage = sinkCurve.Evaluate(percentage);

        Vector3 pos = transform.position;
        pos.y -= sinkDepth * percentage;
        transform.position = pos;

        Move(-docker.transform.forward, driftSpeed * speedModifier);

    }

    private void CheckDistance()
    {
        float sqrDist = (docker.transform.position - transform.position).sqrMagnitude;

        if (sqrDist <= dockingDistance)
        {
            state = BoatState.Docking;
        }
    }

    void Dock()
    {
        timer += Time.deltaTime;

        float percentage = timer / dockingTime;

        if (percentage >= 1f)
        {
            OnDocking();

            return;
        }

        bouyancy.averageFwd = Vector3.Lerp(bouyancy.averageFwd, docker.transform.forward, percentage);
        bouyancy.averageRight = Vector3.Lerp(bouyancy.averageRight, docker.transform.right, percentage);
        transform.position = Vector3.Lerp(transform.position, docker.transform.position, percentage);
    }

    private void OnDocking()
    {
        state = BoatState.Boarding;
        bouyancy.averageFwd = docker.transform.forward;
        bouyancy.averageRight = docker.transform.right;
        transform.position = docker.transform.position;

        timer = 0f;
    }

    private void Move(Vector3 dir, float speed)
    {
        transform.position += dir * speed * Time.deltaTime;
    }
}

public enum BoatState
{
    Moving, Docking, Sinking, Boarding
}
