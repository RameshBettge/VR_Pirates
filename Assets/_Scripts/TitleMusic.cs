using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TitleMusic : MonoBehaviour
{

    int currentSceneIndex;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        CheckScene();
    }


    void CheckScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        if(scene.buildIndex == currentSceneIndex) { return; }

        currentSceneIndex = scene.buildIndex;

        if (scene.name == "FULL GAME")
        {
            Destroy(this);
            Destroy(gameObject);
        }
    }

}
