using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    int pathLength = 50;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 currentPos = new Vector3(Random.Range(250,751),500,0);

        GameObject path = new GameObject();
        path.AddComponent<Cube>();
        path.GetComponent<Cube>().setCubeSize(1f,0.2f,10f);
        path.transform.position = currentPos;

        GameObject[] paths = new GameObject[pathLength];

        for(int i = 0; i < pathLength;i++){
            GameObject branch = new GameObject();
            branch.AddComponent<Cube>();
            branch.AddComponent<BoxCollider>();
            branch.GetComponent<Cube>().setCubeSize(1f,0.2f,10f);
            branch.GetComponent<BoxCollider>().size = new Vector3(1f,0.2f,20f);
            branch.GetComponent<Cube>().setSubmeshIndex(2);
            currentPos += new Vector3(1f,0f,20f);
            branch.transform.position = currentPos;

            paths[i] = branch;
        }
        int randomIndex = Random.Range(0,pathLength);
        GameObject.FindGameObjectWithTag("Player").transform.position = paths[randomIndex].transform.position + new Vector3(0,3,0);
    }
}
