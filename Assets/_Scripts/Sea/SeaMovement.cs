using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMovement : MonoBehaviour
{
    public Texture2D tex;

    [Space(10)]
    [SerializeField]
    float texScale;
    [SerializeField]
    float offsetScale;
    [Space(10)]

    [SerializeField]
    Vector2 moveSpeed = new Vector2(20f, 20f);
    [SerializeField]
    Vector2 offsetSpeed = new Vector2(10f, 10f);
    [Space(10)]

    public float heightModifier = 10f;
    [Space(10)]

    [SerializeField]
    float normalRecalculationInterval = 0.05f;

    //[HideInInspector]
    public float extraMovement;



    float nextTest;

    float halfTexWidth;

    public float movement;
    public float percentage;

    private void Awake()
    {
        halfTexWidth = tex.width;
    }

    public void MoveSea(MeshFilter filter)
    {

        Vector3[] verts = filter.mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            float x = verts[i].x;
            float z = verts[i].z;

            float yPos = GetHeight(x, z);
            verts[i] = new Vector3(verts[i].x, yPos, verts[i].z);
        }

        if (Time.time >= nextTest)
        {
            filter.mesh.RecalculateNormals();
            nextTest = normalRecalculationInterval;
        }

        filter.mesh.vertices = verts;

        // TODO: Check when to recalculate the normals instead of doing it every frame.
    }

    public float GetHeight(Vector3 pos)
    {
        return GetHeight(pos.x, pos.z);

    }

    public float GetHeight(float x, float z)
    {
        float xUV = x * texScale;
        float zUV = z * texScale;

        xUV += moveSpeed.x * Time.time;
        zUV += moveSpeed.y * Time.time;

        float xOffsetUV = x * offsetScale;
        float zOffsetUV = z * offsetScale;

        xOffsetUV += offsetSpeed.x * Time.time;
        zOffsetUV += offsetSpeed.y * Time.time;

        xOffsetUV += extraMovement;
        xUV += extraMovement;

        // movement gets stuck after hitting 2040 somehow.
        if (movement > halfTexWidth)
        {
            movement -= halfTexWidth;
        }

        // Doesn't work, even though it might improve the soothness of the sea if it would
        //float height = tex.GetPixelBilinear(xUV, zUV).grayscale;

        float height = tex.GetPixel((int)xUV, (int)zUV).grayscale;
        float offset = tex.GetPixel((int)xOffsetUV, (int)zOffsetUV).grayscale;
        offset = (offset - 0.5f) * 2f;

        float yPos = height * heightModifier * offset;
        return yPos;
    }
}
