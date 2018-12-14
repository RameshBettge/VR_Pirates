using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPokerScene : MonoBehaviour
{
    float timeTilSwitch = 30f;

    bool loading = false;

    private void Awake()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Time.timeSinceLevelLoad > timeTilSwitch)
        {
            if (!loading)
            {
                SceneManager.LoadScene("FULL GAME");
                loading = true;
            }
        }
    }
}
