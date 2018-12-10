using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMovement : MonoBehaviour
{
    public Texture2D tex;

    [SerializeField]
    float texScale;
    [SerializeField]
    float moveSpeed = 10f;
    [SerializeField]
    float heightModifier = 10f;

    float movement;


    public void MoveSea(MeshFilter filter)
    {

        Vector3[] verts = filter.mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            int xUV = (int) (verts[i].x * texScale);
            int yUV = (int) (verts[i].z * texScale);

            xUV += (int)movement;
            movement += Time.deltaTime * moveSpeed;

            float height = tex.GetPixel(xUV, yUV).grayscale;

            verts[i] = new Vector3(verts[i].x, height * heightModifier, verts[i].z);
        }

        filter.mesh.vertices = verts;

        // TODO: Check how costly the recalculation is and if it is worth it.
        filter.mesh.RecalculateNormals();
    }

}
