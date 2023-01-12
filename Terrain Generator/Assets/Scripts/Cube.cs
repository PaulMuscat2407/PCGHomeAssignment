using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Cube : MonoBehaviour
{

    private float cubeLength,cubeHeight,cubeWidth;

    private Vector3 size;

    [SerializeField]
    private int subMeshCount = 6;

    int subMeshIndex = 0;

    public Vector3 CubeSize(){
        return size * 2;
    }

    public void setCubeSize(float length,float height,float width){
        cubeLength = length;
        cubeHeight = height;
        cubeWidth = width;
    }

    public void setSubmeshIndex(int index){
        subMeshIndex = index;
    }

    private void CreateCube()
    {

        MeshFilter meshGenerator = this.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshBuilder meshBuilder = new meshBuilder(subMeshCount);

        //Specify Points and Triangles
        Vector3 t0 = new Vector3(size.x, size.y, -size.z);
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 t2 = new Vector3(-size.x, size.y, size.z);
        Vector3 t3 = new Vector3(size.x, size.y, size.z);

        Vector3 b0 = new Vector3(size.x, -size.y, -size.z);
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z);
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z);
        Vector3 b3 = new Vector3(size.x, -size.y, size.z);

        //Top Square Face
        meshBuilder.generateTriangleMesh(t0, t1, t2, subMeshIndex);
        meshBuilder.generateTriangleMesh(t0, t2, t3, subMeshIndex);

        //Bottom Square Face
        meshBuilder.generateTriangleMesh(b2, b1, b0, subMeshIndex);
        meshBuilder.generateTriangleMesh(b3, b2, b0, subMeshIndex);

        //Front Square Face
        meshBuilder.generateTriangleMesh(t0, b0, b1, subMeshIndex);
        meshBuilder.generateTriangleMesh(t0, b1, t1, subMeshIndex);

        //Back Square Face
        meshBuilder.generateTriangleMesh(t3, b2, b3, subMeshIndex);
        meshBuilder.generateTriangleMesh(t3, t2, b2, subMeshIndex);

        //Left Square Face
        meshBuilder.generateTriangleMesh(b1, b2, t1, subMeshIndex);
        meshBuilder.generateTriangleMesh(t1, b2, t2, subMeshIndex);

        //Right Square Face
        meshBuilder.generateTriangleMesh(b3, b0, t3, subMeshIndex);
        meshBuilder.generateTriangleMesh(t0, t3, b0, subMeshIndex);

        meshGenerator.mesh = meshBuilder.CreateMesh();

        materialBuilder materialsBuilder = new materialBuilder();

        meshRenderer.materials = materialsBuilder.MaterialsList().ToArray();
    }

    private void Start() {
        size = new Vector3(cubeLength,cubeHeight,cubeWidth);

        CreateCube();
    }
}
