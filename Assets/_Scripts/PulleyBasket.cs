using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyBasket : MonoBehaviour
{
    [SerializeField]
    Cannon cannon;

    [SerializeField]
    Throwing throwing;


    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Triggered");

        if (other.CompareTag("PufferFish"))
        {
            string theName = other.gameObject.name; 
            GrabbableObject grabbable = other.GetComponent<GrabbableObject>();
            if (grabbable != null && grabbable.isGrabbed == false)
            {
                Destroy(other.gameObject);
                cannon.stock++;

                //Debug.Log(cannon.stock);
            }
           
        }
        else if (other.CompareTag("Bucket"))
        {
            GrabbableObject grabbable = other.GetComponent<GrabbableObject>();

            if (grabbable != null && grabbable.isGrabbed == false)
            {
                Bucket bucket = GetComponent<Bucket>();

                if(bucket == null)
                {
                    Debug.LogError(other.name + " has Bucket-Tag but no bucket script attached.");
                    return;
                }

                if (!bucket.filled) { return; }

                Destroy(other.gameObject);
                throwing.stock++;

                Debug.Log("Added water." + cannon.stock);
            }
        }
    }
}
