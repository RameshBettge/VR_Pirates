using UnityEngine;
using UnityEngine.SceneManagement;

public class TextBlender : MonoBehaviour
{
    public const string level101name = "Poker";
    public Transform[] sentences;
    float NextDisplay;
    int index = 0;

    bool loading = false;

    void Start()
    {
        NextDisplay = Time.time;
    }

    private void Update()
    {
        Blend();
    }

    void Blend()
    {
        if (Time.time > NextDisplay || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            NextDisplay =  Time.time + 7f;

            if(index != 0)
            {
                sentences[index -1].gameObject.SetActive(false);
            }


            if(index == sentences.Length && !loading)
            {
                loading = true;
                // Remove all ui -> load game after 15f;
                SceneManager.LoadScene("Poker");
            }
            sentences[index].gameObject.SetActive(true);
            index++;
        }
    }
}