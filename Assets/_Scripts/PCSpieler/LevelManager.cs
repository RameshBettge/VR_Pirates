using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public const string level101name = "Text";

    void OnEnable()
    {
        MenuEvents.Instance.StartPlaying.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Text");
    }

    //public void ChangeScene(string sceneName)
    //{
    //}
}