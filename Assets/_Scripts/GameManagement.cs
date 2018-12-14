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

    [SerializeField]
    AnimationCurve seaRiseCurve;

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
    float environmentMovementSpeed;

    [SerializeField]
    float departTime = 5f;

    [SerializeField]
    float seaRiseHeight = -100f;

    [Space(10)]
    [Header("SeaBehaviour")]
    [SerializeField]
    float seaMovementSpeed;

    [SerializeField]
    float calmSeaHeight = 3f;

    [SerializeField]
    float stormySeaHeight = 8f;

    [SerializeField]
    float calmMaterialHeight = 8f;

    [SerializeField]
    float stormyMaterialheight = 60f;

    [Space(20)]
    [Header("SeaColors")]

    [SerializeField]
    Renderer seaPlane;

    [SerializeField]
    AnimationCurve colorChangeCurve;

    [SerializeField]
    SeaColors startColors;
    [SerializeField]
    SeaColors openSeaColors;
    [SerializeField]
    SeaColors ghostHarborColors;

    SeaColors lastColors;
    SeaColors nextColors;

    Material seaMat;

    float startSeaHight;


    //[SerializeField]
    //float boatSinkLimit = 5f;

    float harborPhaseEnd;
    float seaApproachEnd;
    float seaPhaseEnd;
    float ghostHarborPhaseEnd;

    SkeletonBoatSpawner boatSpawner;
    EnemyManager harborSkeletonSpawner;
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

        seaMat = sea.GetComponent<Renderer>().material;

        seaMat.SetFloat("_Height", calmMaterialHeight);

        startSeaHight = sea.transform.position.y;

        seaMat.SetColor("_TopColor", startColors.TopColor);
        seaMat.SetColor("_BottomColor", startColors.BottomColor);
        seaPlane.material.color = startColors.BottomColor;
        RenderSettings.fogColor = startColors.FogColor;
        RenderSettings.fogDensity = startColors.fogDensity;
        lastColors = startColors;
        nextColors = openSeaColors;
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
        MoveEnvironment(harborPhaseEnd, seaApproachEnd, false);

        if (Time.time > seaApproachEnd)
        {
            state = GameState.OnSea;
            islandManager.DoSetActive(false);
            lastColors = openSeaColors;
            nextColors = ghostHarborColors;
        }
    }

    void ChangeSeaColor(float percentage)
    {
        float adjustedPercentage = colorChangeCurve.Evaluate(percentage);

        seaMat.SetColor("_TopColor", Color.Lerp(lastColors.TopColor, nextColors.TopColor, adjustedPercentage));

        Color bottomCol = Color.Lerp(lastColors.BottomColor, nextColors.BottomColor, adjustedPercentage);
        seaMat.SetColor("_BottomColor", bottomCol);
        seaPlane.material.color = bottomCol;
        RenderSettings.fogColor = Color.Lerp(lastColors.FogColor, nextColors.FogColor, adjustedPercentage);
        RenderSettings.fogDensity = Mathf.Lerp(lastColors.fogDensity, nextColors.fogDensity, adjustedPercentage);
    }

    private void MoveEnvironment(float start, float end, bool inverted)
    {
        float percentage = GetAproachPercentage(start, end, true);

        // Has to be uninverted!
        ChangeSeaColor(percentage);

        if (inverted)
        {
            percentage = 1 - percentage;
        }


        float speed = approachCurve.Evaluate(percentage);

        float heightPercentage = seaHeightCurve.Evaluate(percentage);

        float seaRisePercentage = seaRiseCurve.Evaluate(percentage);

        Vector3 seaPos = sea.transform.position;
        seaPos.y = startSeaHight + seaRiseHeight * seaRisePercentage;
        sea.transform.position = seaPos;

        sea.heightModifier = Mathf.Lerp(calmSeaHeight, stormySeaHeight, heightPercentage);
        seaMat.SetFloat("_Height", Mathf.Lerp(calmMaterialHeight, stormyMaterialheight, heightPercentage));


        MoveSea(speed * -seaMovementSpeed);
        MoveAssets(speed * environmentMovementSpeed);
    }

    private void OnSea()
    {
        SeaAttack();
        MoveSea(-seaMovementSpeed);

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
        sea.transform.position += Vector3.left * speed * Time.deltaTime;
        ship.transform.position += Vector3.left * speed * Time.deltaTime;
    }


    private void MoveSea(float speed)
    {
        sea.extraMovement += speed * Time.deltaTime;
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

[System.Serializable]
public struct SeaColors
{
    public Color TopColor;
    public Color BottomColor;
    [Space(5)]
    public Color FogColor;
    public float fogDensity;

}