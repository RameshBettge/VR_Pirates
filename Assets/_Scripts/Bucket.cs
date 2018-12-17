using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField]
    Renderer waterVisual;

    public bool filled { get; private set; }

    bool emptied;

    private void Awake()
    {
        waterVisual.enabled = false;
    }

    public void Fill()
    {
        filled = true;

        waterVisual.enabled = true;
    }

    public void Empty()
    {
        filled = false;

        waterVisual.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!emptied)
        {
            Empty();
        }
    }
}
