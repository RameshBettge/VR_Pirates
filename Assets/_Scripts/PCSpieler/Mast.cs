using UnityEngine;

public class Mast : MonoBehaviour
{
    [SerializeField]
    Camera onLostCam;


    public float health = 2000;
    public GameObject[] masts;
    public GameManagement GameManagement;

    private void Awake()
    {
        onLostCam.gameObject.SetActive(false);
    }

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

                GameManagement.state = GameState.Lost;
            }
        }
    }

    private void OnDisable()
    {
        onLostCam.gameObject.SetActive(true);
    }
}