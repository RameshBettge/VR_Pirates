using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{


    [SerializeField]
    IslandManager islandManager;

    [SerializeField]
    SimpleBouyancy ship;

    [SerializeField]
    AnimationCurve approachCurve;

    [SerializeField]
    AnimationCurve seaHeightCurve;

    [Space(10)]
    [SerializeField]
    float harborSkeletonsDespawnBuffer = 20f;

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
    float environmentMovement;

    [SerializeField]
    float departTime = 5f;

    [Space(10)]
    [Header("SeaBehaviour")]
    [SerializeField]
    float seaMovementSpeed;

    [SerializeField]
    float calmSeaHeight = 3f;

    [SerializeField]
    float stormySeaHeight = 8f;


    //[SerializeField]
    //float boatSinkLimit = 5f;

    float harborPhaseEnd;
    float seaApproachEnd;
    float seaPhaseEnd;
    float ghostHarborPhaseEnd;

    SkeletonBoatSpawner boatSpawner;
    public EnemyManager harborSkeletonSpawner;
    SeaMovement sea;

    public GameState state = GameState.Delay;

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

        sea.heightModifier = calmSeaHeight;
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
        if (Time.time > startDelay)
        {
            state = GameState.Harbor;
            harborSkeletonSpawner.gameObject.SetActive(true);
        }
    }

    private void HarborAttack()
    {
        // Spawn Enemies via Celina's script

        if (Time.time > (harborPhaseEnd - harborSkeletonsDespawnBuffer))
        {
            if (harborSkeletonSpawner.isActiveAndEnabled)
            {
                harborSkeletonSpawner.gameObject.SetActive(false);
            }
            if (Time.time > harborPhaseEnd)
            {
                state = GameState.ApproachingSea;

                ship.OnDepart(departTime);
                //ship.SetHeightLimit(boatSinkLimit);
                ship.enabled = true;
            }
        }
    }

    private void ApproachSea()
    {
        MoveEnvironment(harborPhaseEnd, seaApproachEnd,  false);

        if (Time.time > seaApproachEnd)
        {
            state = GameState.OnSea;
            islandManager.DoSetActive(false);
        }
    }

    private void MoveEnvironment(float start, float end, bool inverted)
    {
        float percentage = GetAproachPercentage(start, end, true);

        if (inverted)
        {
            percentage = 1 - percentage;
        }

        float speed = approachCurve.Evaluate(percentage);

        float heightPercentage = seaHeightCurve.Evaluate(percentage);

        sea.heightModifier = Mathf.Lerp(calmSeaHeight, stormySeaHeight, heightPercentage);

        MoveSea(speed * -seaMovementSpeed);
        MoveAssets(speed * environmentMovement);
    }

    private void OnSea()
    {
        SeaAttack();
        MoveSea(environmentMovement);

        if (Time.time > seaPhaseEnd)
        {
            state = GameState.GhostHarbor;
        }
    }

    private void ApproachGhostHarbor()
    {
        MoveEnvironment(seaPhaseEnd, ghostHarborPhaseEnd, true);

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

    private void MoveAssets(float speed)
    {
        sea.transform.position += Vector3.left * speed;
        ship.transform.position += Vector3.left * speed;
    }


    private void MoveSea(float speed)
    {
        sea.extraMovement += speed;
    }

    float GetAproachPercentage(float start, float end, bool raw)
    {
        end -= start;
        float current = Time.time - start;

        float percentage = current / end;

        if (percentage > 1f)
        {
            percentage = 1f;
        }

        if (!raw)
        {
        percentage = approachCurve.Evaluate(percentage);
        }

        return percentage;
    }
}

public enum GameState
{
    Delay, Harbor, ApproachingSea, OnSea, GhostHarbor, Lost, Won
}