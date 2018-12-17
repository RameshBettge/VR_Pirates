using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulleyBasket : MonoBehaviour
{
    [SerializeField]
    Cannon cannon;

    [SerializeField]
    Throwing throwing;

    [SerializeField]
    int cannonballRestockNumber = 3;

    [SerializeField]
    int waterRestockNumber = 3;

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
                cannon.UpdateStock(cannonballRestockNumber);

                //Debug.Log(cannon.stock);
            }
        }
        else if (other.CompareTag("Bucket"))
        {
            GrabbableObject grabbable = other.GetComponent<GrabbableObject>();

            if (grabbable != null && grabbable.isGrabbed == false)
            {
                Bucket bucket = other.GetComponent<Bucket>();

                if(bucket == null)
                {
                    Debug.LogError(other.name + " has Bucket-Tag but no bucket script attached.");
                    return;
                }

                if (!bucket.filled) { return; }

                Destroy(other.gameObject);
                throwing.UpdateStock(waterRestockNumber);
            }
        }
    }
}
