using UnityEngine;
using UnityEngine.SceneManagement;

public class TextBlender : MonoBehaviour
{
    public const string level101name = "Poker";
    public Transform[] sentences;
    float NextDisplay;
    int index = 0;

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
        if (Time.time > NextDisplay)
        {
            NextDisplay += 7f;

            if(index != 0)
            {
                sentences[index -1].gameObject.SetActive(false);
            }


            if(index == sentences.Length)
            {
                // Remove all ui -> load game after 15f;
                SceneManager.LoadScene("Poker");
            }
            sentences[index].gameObject.SetActive(true);
            index++;
        }
    }
}