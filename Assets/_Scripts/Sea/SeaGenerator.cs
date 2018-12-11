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

    public Material debugMat;

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
            lods[index] = GetBiggestLOD();
            index++;
        }

        for (int i = 0; i < lods.Length; i++)
        {
            float minDistance = 0;
            SeaLOD lastLOD = new SeaLOD(); // won't be used if current lod is smallest one.

            if (i < lods.Length - 1)
            {
                minDistance = lods[i + 1].maxDistanceToBoat;

                lastLOD = lods[i + 1];
            }

            CreateLOD(lods[i], minDistance, i, lastLOD); // TODO: Make sure the vertices that are at 0 on x or z axis aren't created twice.
        }

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

        // Resetting UVs
        Vector2[] uvs = new Vector2[combinedMesh.vertices.Length];
        for (int i = 0; i < combinedMesh.vertices.Length; i++)
        {
            uvs[i] = new Vector2(combinedMesh.vertices[i].x, combinedMesh.vertices[i].z);
        }
        combinedMesh.uv = uvs;

        filter.mesh = combinedMesh;

        //filter.mesh = meshes[0];


        // USED FOR DEBUGGING: Doesn't use combine meshes but instantiates one gameObject per mesh instead. 
        //for (int i = 0; i < meshes.Count; i++)
        //{
        //    Mesh mesh = meshes[i];

        //    GameObject gO = new GameObject();
        //    MeshRenderer rend = gO.AddComponent<MeshRenderer>();
        //    rend.material = debugMat;
        //    MeshFilter filter = gO.AddComponent<MeshFilter>();
        //    filter.mesh = mesh;

        //}
    }

    void CreateLOD(SeaLOD lod, float min, int num, SeaLOD nextSmallerLOD)
    {
        // TODO: This count is just valid for one of the larger quarters.
        //int rowVertexCount = Mathf.CeilToInt((lod.maxDistanceToBoat * 2f) / lod.vertexDensity) + 1; // +1 stands for the vertex at 0.
        //float rawRow = (lod.maxDistanceToBoat * 2f) / lod.vertexDensity);
        int rowVertexCount = Mathf.CeilToInt((lod.maxDistanceToBoat * 2f) / lod.vertexDensity); // No +1 for testing.


        bool middleVertAdded = false;
        // test if a multiple of vertex density equals 1. If so, a vertex has to be added because one will be in the middle.
        float testOne = lod.vertexDensity;
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
            if (testOne >= lod.maxDistanceToBoat)
            {
                rowVertexCount++;
            }
            testOne += lod.vertexDensity;

        }

        int numberofRows = Mathf.CeilToInt((lod.maxDistanceToBoat - min) / lod.vertexDensity + 1); // +1 because the row where two LODs meet, is used by both. // +1 again for testing
        int totalVertices = rowVertexCount * numberofRows;


        float increment = lod.vertexDensity;
        if (num == lods.Length - 1)
        {
            // Middle Square
            CreateSection(lod, -lod.maxDistanceToBoat, rowVertexCount, numberofRows * 2 - 1, totalVertices * 2 - rowVertexCount, increment);
        }
        else
        {
            // Front
            CreateSection(lod, min, rowVertexCount, numberofRows, totalVertices, increment);
            // Back
            CreateSection(lod, -min, rowVertexCount, numberofRows, totalVertices, increment, true);

            // Adjust counts so that the sections on the side don't overlap with the wider sections of the same LOD.
            rowVertexCount = Mathf.CeilToInt((nextSmallerLOD.maxDistanceToBoat * 2f) / lod.vertexDensity);
            totalVertices = rowVertexCount * numberofRows;

            // Right
            CreateSection(lod, -nextSmallerLOD.maxDistanceToBoat, rowVertexCount, numberofRows, totalVertices, increment, true, true);

            // Left
            CreateSection(lod, -nextSmallerLOD.maxDistanceToBoat, rowVertexCount, numberofRows, totalVertices, increment, false, true);

        }
    }

    private void CreateSection(SeaLOD lod, float min, int rowVertexCount, int numberofRows, int totalVertices, float increment, bool flipped = false, bool side = false)
    {


        float end = lod.maxDistanceToBoat;
        float zPos = -lod.maxDistanceToBoat;

        if (side)
        {
            end = -min;
        }

        float xPos = -end;


        Vector3[] currentVertices = new Vector3[totalVertices];

        int index = 0;

        bool isFinished = false;

        int currentRowPos = 1;

        //while (zPos <= -min)
        while (!isFinished)
        {
            if (xPos <= end)
            {
                if(side && flipped)
                {
                    AddVertex(-zPos, xPos);
                    currentVertices[index] = new Vector3(-zPos, 0f, xPos);

                }
                else if (side)
                {
                    AddVertex(zPos, xPos);
                    currentVertices[index] = new Vector3(zPos, 0f, xPos);
                }
                else if (flipped)
                {
                    AddVertex(xPos, -zPos);
                    currentVertices[index] = new Vector3(xPos, 0f, -zPos);
                }
                else
                {
                    AddVertex(xPos, zPos);
                    currentVertices[index] = new Vector3(xPos, 0f, zPos);

                }
                index++;
                xPos += Mathf.Abs(increment);

                currentRowPos++;
                if (currentRowPos == rowVertexCount)
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
                xPos = -end;
            }

            //if (zPos > -min)
            if (index == totalVertices - rowVertexCount)
            {
                if (side || flipped)
                {
                    zPos = min;
                }
                else
                {
                    zPos = -min;
                }
            }
        }

        if (side) { flipped = !flipped; }

        CreateMesh(currentVertices, rowVertexCount, numberofRows, flipped);
    }

    void CreateMesh(Vector3[] verts, int rowVertexCount, int numberOfRows, bool flipped)
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


                if (flipped)
                {
                    triangles[tri] = vert;
                    triangles[tri + 2] = vert + rowVertexCount;
                    triangles[tri + 1] = vert + 1;

                    triangles[tri + 3] = vert + 1;
                    triangles[tri + 5] = vert + rowVertexCount;
                    triangles[tri + 4] = vert + rowVertexCount + 1;
                }
                else
                {
                    triangles[tri] = vert;
                    triangles[tri + 1] = vert + rowVertexCount;
                    triangles[tri + 2] = vert + 1;

                    triangles[tri + 3] = vert + 1;
                    triangles[tri + 4] = vert + rowVertexCount;
                    triangles[tri + 5] = vert + rowVertexCount + 1;

                }
            }
        }

        mesh.triangles = triangles;

        //filter.mesh = mesh;
        meshes.Add(mesh);
    }

    void AddVertex(float x, float z)
    {
        vertices.Add(new Vector3(x, 0f, z));
    }


    SeaLOD GetBiggestLOD()
    {
        if (lodList.Count < 1)
        {
            Debug.LogError("Cannot return least seaLOD - List is empty.");
        }

        float most = 0;
        SeaLOD biggestLOD = new SeaLOD();

        for (int i = 0; i < lodList.Count; i++)
        {
            if (i == 0 || lodList[i].maxDistanceToBoat > most)
            {
                most = lodList[i].maxDistanceToBoat;
                biggestLOD = lodList[i];
            }
        }

        lodList.Remove(biggestLOD);

        return biggestLOD;
    }

    private void Update()
    {
        movement.MoveSea(filter);
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) { return; }

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vert = vertices[i];
            vert.y = -i * 0.01f;
            Gizmos.DrawSphere(vert, 0.1f);
        }

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