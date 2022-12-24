using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;
    private float xMin,xMax,zMin,zMax;
    void Start()
    {
        Vector3 randomPosition = GetRandomPosition();
        this.transform.position = randomPosition;
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(xMin, terrain.terrainData.size.x);
        float z = Random.Range(zMin, terrain.terrainData.size.z);
        return new Vector3(x, 500, z);
    }
}
