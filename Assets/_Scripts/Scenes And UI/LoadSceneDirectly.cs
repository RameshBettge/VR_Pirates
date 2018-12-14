using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneDirectly : MonoBehaviour
{
    bool loading = false;

    public void LoadScene(string sceneToLoad)
    {
        if(SceneManager.GetSceneByName(sceneToLoad) == null)
        {
            Debug.LogError(sceneToLoad + " cannot be loaded - it doesn't exist in build!");
        }

        if (!loading)
        {
            loading = true;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
