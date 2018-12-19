using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWindowed : MonoBehaviour
{
    public FullScreenMode screenMode;

    private void Awake()
    {
        Screen.fullScreenMode = screenMode;
    }

}
