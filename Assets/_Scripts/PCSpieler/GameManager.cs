using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelManager levelManager;
    public LevelManager LevelManager
    {
        get
        {
            return levelManager;
        }
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                instance.levelManager = instance.GetComponentInChildren<LevelManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
        private set { instance = value; }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        levelManager = GetComponentInChildren<LevelManager>();
    }
}
