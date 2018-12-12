using System.Collections.Generic;
using UnityEngine;

public class EditorPath : MonoBehaviour
{
    public Color rayColor = Color.black;
    public List<Transform> pathObjs = new List<Transform>();
    Transform[] theArray;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform>();
        pathObjs.Clear();

        foreach (Transform pathObj in theArray)
        {
            if (pathObj != this.transform)
            {
                pathObjs.Add(pathObj);
            }
        }

        for (int i = 0; i < pathObjs.Count; i++)
        {
            Vector3 pos = pathObjs[i].position;
            if (i > 0)
            {
                Vector3 prev = pathObjs[i - 1].position;
                Gizmos.DrawLine(prev, pos);
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }
}