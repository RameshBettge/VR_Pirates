using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField]
    Renderer waterVisual;

    [SerializeField]
    float extinguishRadius;

    [SerializeField]
    LayerMask targetMask;

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

    void TryToExtinguish()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, extinguishRadius, targetMask);

        for (int i = 0; i < cols.Length; i++)
        {
            Target target = cols[i].GetComponent<Target>();

            if(target != null)
            {
                target.Extinguish();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!emptied && filled)
        {
            TryToExtinguish();
            Empty();
            emptied = true;
        }
    }
}
