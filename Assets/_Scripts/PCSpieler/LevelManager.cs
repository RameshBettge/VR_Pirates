using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public const string level101name = "01";

    void OnEnable()
    {
        MenuEvents.Instance.StartPlaying.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("01");
    }

    //public void ChangeScene(string sceneName)
    //{
    //}
}