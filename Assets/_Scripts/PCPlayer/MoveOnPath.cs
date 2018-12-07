using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    public EditorPath pathToFollow;
    public int currentWayPointID = 0;
    public float speed;
    public float rotationSpeed = 5.0f;

    float reachDistance = 1.0f;

    Vector3 lastPosition;
    Vector3 currentPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (currentWayPointID <= pathToFollow.pathObjs.Count - 1)
        {
            float distance = Vector3.Distance(pathToFollow.pathObjs[currentWayPointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, pathToFollow.pathObjs[currentWayPointID].position, Time.deltaTime * speed);
            var rotation = Quaternion.LookRotation(pathToFollow.pathObjs[currentWayPointID].position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            if (distance <= reachDistance)
            {
                currentWayPointID++;
            }
        }
    }
}