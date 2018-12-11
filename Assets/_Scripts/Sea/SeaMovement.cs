using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMovement : MonoBehaviour
{
    public Texture2D tex;

    [SerializeField]
    float texScale;
    [SerializeField]
    float offsetScale;
    [SerializeField]
    Vector2 moveSpeed = new Vector2(20f, 20f);
    [SerializeField]
    Vector2 offsetSpeed = new Vector2(10f, 10f);

    [SerializeField]
    float heightModifier = 10f;



    float lastTest;

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

            float xUV = x * texScale;
            float zUV = z * texScale;

            xUV += moveSpeed.x * Time.time;
            zUV += moveSpeed.y * Time.time;

            float xOffsetUV = x * offsetScale;
            float zOffsetUV = z * offsetScale;

            xOffsetUV += offsetSpeed.x * Time.time;
            zOffsetUV += offsetSpeed.y * Time.time;


            //movement += Time.deltaTime * moveSpeed;


            // movement gets stuck after hitting 2040 somehow.
            if (movement > halfTexWidth)
            {
                movement -= halfTexWidth;
            }

            //float height = tex.GetPixelBilinear(xUV, zUV).grayscale;

            float height = tex.GetPixel((int)xUV, (int)zUV).grayscale;
            float offset = tex.GetPixel((int)xOffsetUV, (int)zOffsetUV).grayscale;
            offset = (offset - 0.5f) * 2f;

            //verts[i] = new Vector3(verts[i].x, height * heightModifier, verts[i].z);
            verts[i] = new Vector3(verts[i].x, height * heightModifier * offset, verts[i].z);

            //if (Time.time > lastTest + 5f)
            //{
            //    Debug.Log("Working... " + movement);
            //    lastTest = Time.time;
            //}
        }

        filter.mesh.vertices = verts;

        // TODO: Check when to recalculate the normals instead of doing it every frame.
        filter.mesh.RecalculateNormals();
    }

}
