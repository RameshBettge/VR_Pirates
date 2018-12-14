﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour {

    [SerializeField]
    GameObject[] stage1Decorations;

    [SerializeField]
    GameObject stage1;

    [SerializeField]
    GameObject stage3;

    private void Awake()
    {
        stage3.SetActive(false);
    }

    public void DisableStage1Deco()
    {
        for (int i = 0; i < stage1Decorations.Length; i++)
        {
            stage1Decorations[i].SetActive(false);
        }
    }

    public void DoSetActive(bool active)
    {
        transform.GetChild(0).gameObject.SetActive(active);
    }
}