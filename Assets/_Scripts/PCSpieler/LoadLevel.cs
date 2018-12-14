using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public const string level101name = "FULL GAME";

    void OnEnable()
    {
        MenuEvents.Instance.StartPlaying.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("FULL GAME");
    }
}