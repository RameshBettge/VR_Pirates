using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyBasket : MonoBehaviour
{
    [SerializeField]
    Cannon cannon;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PufferFish"))
        {
            if (other.GetComponent<GrabbableObject>().isGrabbed == false)
            {
                Destroy(other.gameObject);
                cannon.stock++;
            }
            else
            {
                // check if bucket which is full
            }
        }
    }
}
