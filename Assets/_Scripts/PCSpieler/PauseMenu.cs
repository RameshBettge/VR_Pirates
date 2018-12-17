using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public PCPlayerMovement pcPlayerScript;
    public Throwing throwingScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (pcPlayerScript.enabled == false)
        {
            pcPlayerScript.enabled = true;
        }
        if (throwingScript.enabled == false)
        {
            throwingScript.enabled = true;
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        if (pcPlayerScript.enabled == true)
        {
            pcPlayerScript.enabled = false;
        }
        if (throwingScript.enabled == true)
        {
            throwingScript.enabled = false;
        }
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}