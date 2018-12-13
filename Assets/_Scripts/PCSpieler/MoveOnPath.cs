using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    Skeleton skeleton;

    public EditorPath pathToFollow;
    public int currentWayPointID = 0;
    public float speed;
    public float rotationSpeed = 5f;

    float reachDistance = 1f;

    Vector3 lastPosition;
    Vector3 currentPosition;

    void Start()
    {
        skeleton = GetComponent<Skeleton>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!skeleton.boarded) { return; }

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