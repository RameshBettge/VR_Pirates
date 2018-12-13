using UnityEngine;

public class Mast : MonoBehaviour
{
    public float health = 2000;
    public GameObject[] masts;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            health -= 20;
            if (health <= 0)
            {
                masts = GameObject.FindGameObjectsWithTag("Target");
                for (int i = 0; i < masts.Length; i++)
                {
                    Destroy(masts[i]);
                }
            }
        }
    }
}