using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SeaMovement))]
public class SeaGenerator : MonoBehaviour
{
    [SerializeField]
    List<SeaLOD> lodList;


    SeaLOD[] lods;

    SeaMovement movement;

    MeshRenderer rend;
    MeshFilter filter;

    List<Mesh> meshes;


    List<Vector3> vertices;

    public int debug = 0;

    void Start()
    {
        meshes = new List<Mesh>();

        movement = GetComponent<SeaMovement>();

        rend = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();

        lods = new SeaLOD[lodList.Count];
        vertices = new List<Vector3>();

        int index = 0;
        while (lodList.Count > 0) // bring LODs in order
        {
            lods[index] = GetLeastLOD();
            index++;
        }

        for (int i = 0; i < lods.Length; i++)
        {
            float minDistance = 0;

            if (i < lods.Length - 1) { minDistance = lods[i + 1].maxDistanceToBoat; }

            CreateLOD(lods[i], minDistance, i); // TODO: Make sure the vertices that are at 0 on x or z axis aren't created twice.
        }


        ////CreateLOD(lods[0], lods[1].maxDistanceToBoat, 0);
        //CreateLOD(lods[1], lods[2].maxDistanceToBoat, 1);
        ////CreateLOD(lods[2], 0, 2);


        CreateSea();
    }

    private void CreateSea()
    {
        CombineInstance[] combines = new CombineInstance[meshes.Count];
        for (int i = 0; i < combines.Length; i++)
        {
            combines[i] = new CombineInstance();
            combines[i].mesh = meshes[i];
            // NOTE: maybe the combines[i].transform has to be set.
            combines[i].transform = transform.localToWorldMatrix;
        }
        Mesh combinedMesh = new Mesh();

        combinedMesh.CombineMeshes(combines);
        filter.mesh = combinedMesh;

        //filter.mesh = meshes[0];
    }

    void CreateLOD(SeaLOD lod, float min, int num)
    {
        // TODO: This count is just valid for one of the larger quarters.
        //int rowVertexCount = Mathf.CeilToInt((lod.maxDistanceToBoat * 2f) / lod.vertexDensity) + 1; // +1 stands for the vertex at 0.
        //float rawRow = (lod.maxDistanceToBoat * 2f) / lod.vertexDensity);
        int rowVertexCount = Mathf.CeilToInt((lod.maxDistanceToBoat * 2f) / lod.vertexDensity); // No +1 for testing.


        bool middleVertAdded = false;
        // test if a multiple of vertex density equals 1. If so, a vertex has to be added because one will be in the middle.
        float testOne = lod.vertexDensity;
        //while (testOne < 1.0001f)
        while (testOne < 1.0001f)
        {
            if (testOne >= 1f) // if there is a vertex in the middle
            {
                rowVertexCount++;
                middleVertAdded = true;
            }

            testOne += lod.vertexDensity;
        }
        while (testOne < lod.maxDistanceToBoat + 0.001f && !middleVertAdded)
        {
            if(testOne >= lod.maxDistanceToBoat)
            {
                rowVertexCount++;
            }
            testOne += lod.vertexDensity;

        }


        // some other numbers have a vertex in the middle as well, but that may depend on the relation to the maxDistance to boat.
        //if ((lod.vertexDensity == 2 || lod.vertexDensity == 5) && lod.vertexDensity != 6 && lod.vertexDensity != 0.4f) { rowVertexCount++; }

        //int numberofRows = Mathf.FloorToInt((lod.maxDistanceToBoat - min) / lod.vertexDensity + 1); // +1 because the row where two LODs meet, is used by both. // +1 again for testing
        int numberofRows = Mathf.CeilToInt((lod.maxDistanceToBoat - min) / lod.vertexDensity + 1); // +1 because the row where two LODs meet, is used by both. // +1 again for testing
        int totalVertices = rowVertexCount * numberofRows;

        float xPos = -lod.maxDistanceToBoat;
        int end = lod.maxDistanceToBoat;

        float zPos = -lod.maxDistanceToBoat;

        float increment = lod.vertexDensity;

        Vector3[] currentVertices = new Vector3[totalVertices];

        int index = 0;

        bool isFinished = false;

        int currentRowPos = 1;

        while (!isFinished)
        //while (zPos <= -min)
        {
            if (xPos <= end)
            {
                AddVertex(xPos, zPos);
                currentVertices[index] = new Vector3(xPos, 0f, zPos);
                index++;
                xPos += increment;

                currentRowPos++;
                if(currentRowPos == rowVertexCount)
                {
                    xPos = end;
                    currentRowPos = 0;
                }
            }
            else
            {
                //if (zPos == -min)
                if (index >= totalVertices - 1) // have while loop until index reaches totalvertices -1 instead.
                {
                    isFinished = true;
                }
                zPos += increment;
                xPos = -lod.maxDistanceToBoat;
            }

            //if (zPos > -min)
            if (index == totalVertices - rowVertexCount || zPos > -min)
            {
                zPos = -min;
            }
        }

        if (!isFinished) { zPos = -min; }

        CreateMesh(currentVertices, rowVertexCount, numberofRows);

    }


    void CreateMesh(Vector3[] verts, int rowVertexCount, int numberOfRows)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verts;

        int faceCount = (rowVertexCount - 1) * (numberOfRows - 1);
        int[] triangles = new int[faceCount * 6];

        int tri = 0;

        for (int i = 0; i < numberOfRows - 1; i++)
        {
            for (int j = 0; j < rowVertexCount - 1; j++, tri += 6)
            {
                int vert = rowVertexCount * i + j;

                triangles[tri] = vert;
                triangles[tri + 1] = vert + rowVertexCount;
                triangles[tri + 2] = vert + 1;

                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + rowVertexCount;
                triangles[tri + 5] = vert + rowVertexCount + 1;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        //filter.mesh = mesh;
        meshes.Add(mesh);
    }

    void AddVertex(float x, float z)
    {
        vertices.Add(new Vector3(x, 0f, z));
    }


    SeaLOD GetLeastLOD()
    {
        if (lodList.Count < 1)
        {
            Debug.LogError("Cannot return least seaLOD - List is empty.");
        }

        float most = 0;
        SeaLOD leastLOD = new SeaLOD();

        for (int i = 0; i < lodList.Count; i++)
        {
            if (i == 0 || lodList[i].maxDistanceToBoat > most)
            {
                most = lodList[i].maxDistanceToBoat;
                leastLOD = lodList[i];
            }
        }

        lodList.Remove(leastLOD);

        return leastLOD;
    }

    private void Update()
    {
        movement.MoveSea(filter);
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) { return; }

        //for (int i = 0; i < vertices.Count; i++)
        //{
        //    Gizmos.DrawSphere(vertices[i], 0.1f);
        //}

        //Gizmos.color = Color.blue;
        //for (int i = 0; i < filter.mesh.vertices.Length; i++)
        //{
        //    Gizmos.DrawWireSphere(filter.mesh.vertices[i], 1f);
        //}
        Gizmos.color = Color.magenta;

        //Gizmos.DrawWireSphere(filter.mesh.vertices[debug], 1f);
    }
}




[System.Serializable]
public struct SeaLOD
{
    public int maxDistanceToBoat;
    [Tooltip("Should add up to 1 or be an integer.")]
    public float vertexDensity;
}