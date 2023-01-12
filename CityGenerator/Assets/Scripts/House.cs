using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private int houseLength,houseWidth,houseHeight;
    private Vector3 position;
    private Quaternion rotation;
    void Start()
    {
        CreateHouse();
        this.transform.position = position;
        this.transform.rotation = rotation;
    }

    void Update(){

        
        RaycastHit hit;

        //Checks if Road is under the houses and removes them
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.tag == "Road")
            {
                Destroy(gameObject);
            }
        }

        //Checks if Houses are clipping, and removes them.
        if (Physics.Raycast(transform.position,Vector3.forward, out hit,1.02f) || Physics.Raycast(transform.position, Vector3.back, out hit,1.02f) ||Physics.Raycast(transform.position,Vector3.left, out hit,1.02f)||Physics.Raycast(transform.position,Vector3.right, out hit,1.02f))
        {
            if (hit.collider.tag == "House")
            {
                Destroy(gameObject);
            }
        }
    }

    public void setHouseSize(int length,int height,int width){
        houseLength = length;
        houseHeight = height;
        houseWidth = width;
    }

    public void setPos(Vector3 pos){
        position = pos;
    }

    public int GetLength(){
        return houseLength;
    }
    public int GetHeight(){
        return houseHeight;
    }
    public int GetWidth(){
        return houseWidth;
    }

    public Vector3 returnHouseSize(){
        return new Vector3(houseLength*2,houseHeight*2,houseWidth*2);
    }

    //Randomly Chooses Left or Right Side of the roads
    public float GetSide(float position){
        if(Random.value < 0.5f){
            position += 4f;
        }else{
            position-= 4f;
        }

        return position;
    }

    //Creates the house.
    private void CreateHouse(){
        GameObject house = new GameObject();
        house.name = "Building";
        house.transform.parent = this.transform;
        house.AddComponent<Cube>();
        house.GetComponent<Cube>().setCubeSize(houseLength,houseHeight,houseWidth);
        house.GetComponent<Cube>().setSubmeshIndex(3);

        GameObject door = new GameObject();
        door.transform.parent = this.transform;
        door.transform.position = new Vector3(-houseLength + 0.05f,-houseHeight + 0.5f, 0);
        door.AddComponent<Cube>();
        door.GetComponent<Cube>().setCubeSize(0.2f,1.4f,0.5f);
        door.GetComponent<Cube>().setSubmeshIndex(2);

        GameObject backDoor = new GameObject();
        backDoor.transform.parent = this.transform;
        backDoor.transform.position = new Vector3(houseLength - 0.05f,-houseHeight + 0.5f, 0);
        backDoor.AddComponent<Cube>();
        backDoor.GetComponent<Cube>().setCubeSize(0.2f,1.4f,0.5f);
        backDoor.GetComponent<Cube>().setSubmeshIndex(2);

        GameObject WindowFB = new GameObject();
        WindowFB.transform.parent = this.transform;
        WindowFB.transform.position = new Vector3(0,0.8f, 0);
        WindowFB.AddComponent<Cube>();
        WindowFB.GetComponent<Cube>().setCubeSize(houseLength+0.05f,0.5f,0.7f);
        WindowFB.GetComponent<Cube>().setSubmeshIndex(1);

        GameObject WindowS = new GameObject();
        WindowS.transform.parent = this.transform;
        WindowS.transform.position = new Vector3(0,0.8f, 0);
        WindowS.AddComponent<Cube>();
        WindowS.GetComponent<Cube>().setCubeSize(0.7f,0.5f,houseWidth+0.05f);
        WindowS.GetComponent<Cube>().setSubmeshIndex(1);

    }
}
