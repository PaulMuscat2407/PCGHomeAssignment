using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshBuilder
{
    //List of Points
    private List<Vector3> vertices = new List<Vector3>();

    //Index to a list of Vertices to specify which points connect
    private List<int> vertexIndex = new List<int>();

    //A list that defines the directions of each vertex
    private List<Vector3> normals = new List<Vector3>();

    //A lists that contains the positions of uv Maps
    private List<Vector2> uvMaps = new List<Vector2>();

    //Array of submesh Indices - Array points to an index of a material
    private List<int>[] subMeshIndex = new List<int>[] { };

    public meshBuilder(int subMeshCount){

        subMeshIndex = new List<int>[subMeshCount];

        for(int i = 0; i < subMeshCount; i++){
            subMeshIndex[i] = new List<int>();
        }

    }

    public void generateTriangleMesh(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;

        generateTriangleMesh(p0, p1, p2, normal, subMesh);
    }

    public void generateTriangleMesh(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int subMesh)
    {

        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        vertexIndex.Add(p0Index);
        vertexIndex.Add(p1Index);
        vertexIndex.Add(p2Index);

        subMeshIndex[subMesh].Add(p0Index);
        subMeshIndex[subMesh].Add(p1Index);
        subMeshIndex[subMesh].Add(p2Index);

        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);

        for (int i = 0; i < 3; i++)
        {
            normals.Add(normal);
        }

        uvMaps.Add(new Vector2(0, 0));
        uvMaps.Add(new Vector2(0, 1));
        uvMaps.Add(new Vector2(1, 1));
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = vertexIndex.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvMaps.ToArray();

        mesh.subMeshCount = subMeshIndex.Length;

        for(int i = 0; i< subMeshIndex.Length; i++){

            if(subMeshIndex[i].Count < 3){

                mesh.SetTriangles(new int[3] {0,0,0},i);

            }
            mesh.SetTriangles(subMeshIndex[i].ToArray(),i);
        }

        return mesh;
    }

}
