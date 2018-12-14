using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScence : MonoBehaviour
{
    public const string level101name = "Poker";

    void OnEnable()
    {
        MenuEvents.Instance.StartPlaying.AddListener(ChangeScene);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Poker");
    }
}