using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    [SerializeField]
    AnimationCurve approachCurve;

    [Header("Timers")]
    [SerializeField]
    float startDelay = 5f;

    [SerializeField]
    float harborPhaseDuration = 5f;

    [SerializeField]
    float seaApproachDuration = 5f;

    [SerializeField]
    float seaPhaseDuration = 5f;

    [SerializeField]
    float ghostHarborPhaseDuration = 5f;

    [Space(10)]

    [SerializeField]
    float seaMovementSpeed;

    float harborPhaseEnd;
    float seaApproachEnd;
    float seaPhaseEnd;
    float ghostHarborPhaseEnd;

    SkeletonBoatSpawner boatSpawner;
    EnemyManager harborSkeletonSpawner;
    SeaMovement sea;

    public GameState state = GameState.Delay;

    public float debug;

    void Start()
    {
        harborPhaseEnd = startDelay + harborPhaseDuration;
        seaApproachEnd = harborPhaseEnd + seaApproachDuration;
        seaPhaseEnd = seaApproachEnd + seaPhaseDuration;
        ghostHarborPhaseEnd = seaPhaseEnd + ghostHarborPhaseDuration;

        boatSpawner = GetComponentInChildren<SkeletonBoatSpawner>(true);
        harborSkeletonSpawner = GetComponentInChildren<EnemyManager>(true);
        sea = GetComponentInChildren<SeaMovement>(true);

        boatSpawner.gameObject.SetActive(false);
        harborSkeletonSpawner.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (state)
        {
            case GameState.Delay:
                Delay();
                break;

            case GameState.Harbor:
                HarborAttack();
                break;

            case GameState.ApproachingSea:
                ApproachSea();
                break;

            case GameState.OnSea:
                OnSea();
                break;

            case GameState.GhostHarbor:
                ApproachGhostHarbor();
                break;

            case GameState.Lost:
                break;

            case GameState.Won:
                break;

            default:
                break;
        }
    }


    private void Delay()
    {
        if(Time.time > startDelay)
        {
            state = GameState.Harbor;
        }
    }

    private void HarborAttack()
    {
        // Spawn Enemies via Celina's script

        if (Time.time > harborPhaseEnd)
        {
            state = GameState.ApproachingSea;
        }
    }

    private void ApproachSea()
    {
        float percentage = GetAproachPercentage(harborPhaseEnd, seaApproachEnd);
        float speed = percentage * seaMovementSpeed;

        MoveSea(speed);
        MoveHarbor(speed);

        if(Time.time > seaApproachEnd)
        {
            state = GameState.OnSea;
        }
    }

    private void OnSea()
    {
        SeaAttack();
        MoveSea(seaMovementSpeed);

        if (Time.time > seaPhaseEnd)
        {
            state = GameState.GhostHarbor;

            // Increase Spawn interval when entering state and Set the islands position and rotation
        }
    }

    private void ApproachGhostHarbor()
    {
        float percentage = 1 - GetAproachPercentage(seaPhaseEnd, ghostHarborPhaseEnd);
        float speed = percentage * seaMovementSpeed;

        //debug = percentage;

        SeaAttack();

        MoveSea(seaMovementSpeed);
        MoveHarbor(seaMovementSpeed);

        if (Time.time > ghostHarborPhaseEnd)
        {
            state = GameState.Won;
        }
    }

    private void SeaAttack()
    {
        // Spawn Enemy boats
        //throw new NotImplementedException();
    }

    private void MoveHarbor(float speed)
    {
        // Move harbor depending on speed * sea texture scale
        //throw new NotImplementedException();
    }

    private void MoveSea(float speed)
    {
        //throw new NotImplementedException();
    }

    float GetAproachPercentage(float start, float end)
    {
        end -= start;
        float current = Time.time - start;

        float percentage = current / end;

        if (percentage > 1f)
        {
            percentage = 1f;
        }

        percentage = approachCurve.Evaluate(percentage);

        debug = percentage;

        return percentage;
    }
}

public enum GameState
{
    Delay, Harbor, ApproachingSea, OnSea, GhostHarbor, Lost, Won
}