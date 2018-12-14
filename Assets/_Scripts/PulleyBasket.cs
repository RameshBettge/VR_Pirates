using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyBasket : MonoBehaviour
{
    [SerializeField]
    Cannon cannon;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");

        if (other.CompareTag("PufferFish"))
        {
            Debug.Log("Got Puffer");
            if (other.GetComponent<GrabbableObject>().isGrabbed == false)
            {
                Destroy(other.gameObject);
                cannon.stock++;

                Debug.Log(cannon.stock);
            }
            else
            {
                // check if bucket which is full
            }
        }
    }

}
