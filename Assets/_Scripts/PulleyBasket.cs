using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyBasket : MonoBehaviour
{
    [SerializeField]
    Cannon cannon;


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Triggered");

        if (other.CompareTag("PufferFish"))
        {
            string theName = other.gameObject.name; 
            Debug.Log("Got Puffer");
            GrabbableObject grabbable = other.GetComponent<GrabbableObject>();
            if (grabbable != null && grabbable.isGrabbed == false)
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
