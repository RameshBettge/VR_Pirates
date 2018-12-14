using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    float nextSpawn;

    [SerializeField]
    Transform pathParent;

    EditorPath[] paths;
    public GameObject[] skelotons;

    public float spawnInterval = 10f;
    public Transform ship;

    private void Awake()
    {
        paths = pathParent.GetComponentsInChildren<EditorPath>();
    }

    private void OnEnable()
    {
        nextSpawn = Time.timeSinceLevelLoad + spawnInterval;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad> nextSpawn)
        {
            int randomPathNum = Random.Range(0, paths.Length);
            int randomSkelotonNum = Random.Range(0, skelotons.Length);
            EditorPath path = paths[randomPathNum];
            Transform pathParent = path.transform;
            GameObject enemyInstance = Instantiate(skelotons[randomSkelotonNum], pathParent.GetChild(0).position, pathParent.GetChild(0).rotation, ship);
            enemyInstance.GetComponent<MoveOnPath>().pathToFollow = path;
            enemyInstance.GetComponent<Skeleton>().OnBoarding();

            nextSpawn = Time.timeSinceLevelLoad+ spawnInterval;
        }
    }

}