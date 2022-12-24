using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainTextureData
{
    public Texture2D terrainTexture;
    public Vector2 tileSize;
    public float minHeight;
    public float maxHeight;
}

[System.Serializable]

public class TreeData
{

    public GameObject TreeMesh;
    public float minHeight;
    public float maxHeight;
}

public class RandomHeights : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    [SerializeField]
    [Range(0f, 1f)]
    private float minRandomHeight = 0f;

    [SerializeField]
    [Range(0f, 1f)]
    private float maxRandomHeight = 0.1f;

    [SerializeField]
    private bool flattenTerrain = true;

    [Header("Perlin Noise")]
    [SerializeField]
    private bool perlinNoise = false;

    [SerializeField]
    private float perlinNoiseWidthScale = 0.0f;
    [SerializeField]
    private float perlinNoiseHeightScale = 0.1f;

    [Header("Texture Data")]
    [SerializeField]
    private List<TerrainTextureData> terrainTextureData;

    [SerializeField]
    private bool addTerrainTexture;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [Header("Tree Data")]
    [SerializeField]
    private List<TreeData> treeData;

    [SerializeField]
    private int maxTrees = 2000;

    [SerializeField]
    private int treeSpacing = 10;
    [SerializeField]
    private bool addTrees = true;
    [SerializeField]
    private int TerrainLayerIndex;

    [Header("Water")]
    [SerializeField]
    private GameObject Water;

    [SerializeField]
    private float waterHeight = 0.3f;


    [Header("Clouds")]
    [SerializeField]
    private GameObject Cloud;
    [SerializeField]
    private int maxClouds = 10;


    void Start()
    {
        if (terrain == null)
        {
            terrain = this.GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }

        GenerateHeights();
        AddTerrainTextures();
        AddTrees();
        AddWater();
        AddClouds();
    }

    void GenerateHeights()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (perlinNoise)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
                }
                else
                {
                    heightMap[width, height] = Random.Range(minRandomHeight, maxRandomHeight);
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    void FlattenTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] = 0;
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    private void AddTerrainTextures()
    {

        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureData.Count];

        for (int i = 0; i < terrainTextureData.Count; i++)
        {
            if (addTerrainTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureData[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureData[i].tileSize;
            }
            else
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {

                float[] alphaMap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureData.Count; i++)
                {

                    float heightBegin = terrainTextureData[i].minHeight - terrainTextureBlendOffset;
                    float heightEnd = terrainTextureData[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= heightBegin && heightMap[width, height] <= heightEnd)
                    {
                        alphaMap[i] = 1;
                    }

                }
                Blend(alphaMap);

                for (int j = 0; j < terrainTextureData.Count; j++)
                {
                    alphaMapList[width, height, j] = alphaMap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }

    private void Blend(float[] alphaMap)
    {
        float total = 0;

        for (int i = 0; i < alphaMap.Length; i++)
        {
            total += alphaMap[i];
        }

        for (int i = 0; i < alphaMap.Length; i++)
        {
            alphaMap[i] = alphaMap[i] / total;
        }
    }

    private void AddTrees()
    {
        TreePrototype[] trees = new TreePrototype[treeData.Count];

        for (int i = 0; i < treeData.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeData[i].TreeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstances = new List<TreeInstance>();

        if (addTrees)
        {
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {

                for (int x = 0; x < terrainData.size.x; x += treeSpacing)
                {

                    for (int treeIndex = 0; treeIndex < trees.Length; treeIndex++)
                    {

                        if (treeInstances.Count < maxTrees)
                        {

                            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                            if (currentHeight >= treeData[treeIndex].minHeight && currentHeight <= treeData[treeIndex].maxHeight)
                            {

                                float randomX = (x + Random.Range(-5f, 5f)) / terrainData.size.x;
                                float randomZ = (z + Random.Range(-5, 5)) / terrainData.size.z;

                                Vector3 treePosition = new Vector3(randomX * terrainData.size.x, currentHeight * terrainData.size.y, randomZ * terrainData.size.z) + this.transform.position;

                                RaycastHit hit;

                                int layerMask = 1 << TerrainLayerIndex;

                                if (Physics.Raycast(treePosition, -Vector3.up, out hit, 100, layerMask) || Physics.Raycast(treePosition, Vector3.up, out hit, 100, layerMask))
                                {

                                    float treeDistance = (hit.point.y - this.transform.position.y) / terrainData.size.y;

                                    TreeInstance treeInstance = new TreeInstance();

                                    treeInstance.position = new Vector3(randomX, treeDistance, randomZ);
                                    treeInstance.rotation = Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    treeInstances.Add(treeInstance);

                                }
                            }

                        }
                    }
                }
            }
        }
        terrainData.treeInstances = treeInstances.ToArray();
    }

    private void AddWater(){
        GameObject waterGameObject = Instantiate(Water,this.transform.position,this.transform.rotation);
        waterGameObject.name = "Water";
        waterGameObject.transform.position = this.transform.position + new Vector3(terrainData.size.x/2, waterHeight * terrainData.size.y, terrainData.size.z/2);
        waterGameObject.transform.localScale = new Vector3(terrainData.size.x,1,terrainData.size.z);
    }

    private void AddClouds(){
        for(int i = 0; i < maxClouds; i++){
            GameObject cloud = Instantiate(Cloud);
            cloud.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            cloud.transform.position = new Vector3(Random.Range(0,terrainData.size.x),Random.Range(0.9f,1) * terrainData.size.y,Random.Range(0,terrainData.size.z));
        }
    }

    private void OnDestroy()
    {
        if (flattenTerrain)
        {
            FlattenTerrain();
        }
    }
}
