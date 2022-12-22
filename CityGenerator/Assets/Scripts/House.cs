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
        if (Physics.Raycast(transform.position,Vector3.forward, out hit,1f) || Physics.Raycast(transform.position, Vector3.back, out hit,1f) ||Physics.Raycast(transform.position,Vector3.left, out hit,1f)||Physics.Raycast(transform.position,Vector3.right, out hit,1f))
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
    }
}
