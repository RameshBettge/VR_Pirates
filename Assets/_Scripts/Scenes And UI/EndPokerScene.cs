using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPokerScene : MonoBehaviour
{
    float timeTilSwitch = 20f;

    bool loading = false;

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
