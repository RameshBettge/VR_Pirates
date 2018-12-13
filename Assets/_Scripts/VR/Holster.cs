using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holster : MonoBehaviour
{
    [SerializeField]
    Transform pistolPos;

    [SerializeField]
    Transform spawnHelper;

    [SerializeField]
    float timeUntilRespawnComplete = 3f;

    [SerializeField]
    float respawnAnimDuration = 1f;

    float timeUntilRespawnStart;

    float timer = 100f;

    [SerializeField]
    GameObject pistolPrefab;

    BoxCollider[] cols;

    Pistol pistol;
    GrabbableObject grabbable;

    bool pistolDrawn = true;

    bool pistolIsSpawning = false;

    private void Awake()
    {
        timeUntilRespawnStart = timeUntilRespawnComplete - respawnAnimDuration;
    }

    private void Update()
    {
        if (!pistolDrawn) { return; }

        if(!pistolIsSpawning && timer > timeUntilRespawnStart)
        {
            CreatePistol();
            pistolIsSpawning = true;
        }

        if (pistolIsSpawning)
        {
            float percentage = (timer - timeUntilRespawnStart) / respawnAnimDuration;

            percentage = Mathf.Clamp(percentage, 0f, 1f);

            // TODO: Create Animation curve
            spawnHelper.localScale = Vector3.one * percentage;

            if(percentage >= 1f)
            {
                ActivatePistol();
            }
        }

        timer += Time.deltaTime;
    }

    void CreatePistol()
    {
        Debug.Log("Creating pistol");

        GameObject gO = Instantiate(pistolPrefab, pistolPos.position, pistolPos.rotation, spawnHelper);


        cols = gO.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }


        pistol = gO.GetComponent<Pistol>();
        pistol.holster = this;

        //grabbable = gO.GetComponent<GrabbableObject>();
        //pistol.enabled = false;
        //grabbable.enabled = false;
    }

    void ActivatePistol()
    {
        pistolIsSpawning = false;
        pistolDrawn = false;

        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = true;
        }

        Debug.Log("Activating Pistol");
    }

    // TODO: Give pistol reference to this on spawn.

    public void OnDrawPistol()
    {
        pistolDrawn = true;
        timer = 0f;

        Debug.Log("Pistol drawn");
    }


}
