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
            int xUV = (int)(verts[i].x * texScale);
            int yUV = (int)(verts[i].z * texScale);

            xUV += (int)movement;
            movement += Time.deltaTime * moveSpeed;



            // movement gets stuck after hitting 2040 somehow.
            if (movement > halfTexWidth)
            {
                movement -= halfTexWidth;
            }

            float height = tex.GetPixel(xUV, yUV).grayscale;

            verts[i] = new Vector3(verts[i].x, height * heightModifier, verts[i].z);



            // Todo:    If height == 0.5f -> 0 in local space. 
            //          Values should be inverted based on sinus-function (first white = 1, black = -1 ; then the other way round.)
            //          Problem:    
            //                      if the sinus function returns 0, the sea will be perfectly flat. 
            //
            //          Possible Solution:
            //                      Maybe this change should be offset with noise,
            //                      or only occur in an area indicated by another texture (e.g. the HalfTransparentSmooth which scrolls over the sea.)

            //percentage = Mathf.Sin(movement * 0.1f);
            //percentage = (percentage + 0.5f) * 0.5f;

            //height = Mathf.Lerp(height, -height, percentage);

            //verts[i] = new Vector3(verts[i].x, height * heightModifier * percentage, verts[i].z);


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
