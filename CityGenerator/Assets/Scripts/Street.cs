using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    private int streetLength;
    private Vector3 position;
    private Quaternion rotation;

    private void Start() {
        CreateStreet();
        this.transform.position = position;
        this.transform.rotation = rotation;
    }

    public void setStreetLength(int length){
        streetLength = length/2;
    }

    public int getStreetLength(){
        return streetLength;
    }

    public void setPos(Vector3 pos){
        position = pos;
    }

    public void setRotation(Quaternion rot){
        rotation = rot;
    }

    private void CreateStreet(){

        GameObject divider = new GameObject();
        divider.name = "MiddleStrip";
        divider.transform.parent = this.transform;
        divider.AddComponent<Cube>();
        divider.GetComponent<Cube>().setCubeSize(streetLength,0.05f,0.1f);
        divider.GetComponent<Cube>().setSubmeshIndex(1);

        for(int i = 0;i < 2;i++){
            GameObject street = new GameObject();
            street.transform.parent = this.transform;
            street.AddComponent<Cube>();
            street.GetComponent<Cube>().setCubeSize(streetLength,0.05f,0.5f);
            street.GetComponent<Cube>().setSubmeshIndex(0);

            GameObject pavement = new GameObject();
            pavement.transform.parent = this.transform;
            pavement.AddComponent<Cube>();
            pavement.GetComponent<Cube>().setCubeSize(streetLength,0.05f,0.3f);
            pavement.GetComponent<Cube>().setSubmeshIndex(2);

            switch(i){
                case 0:
                    street.name = "Street Right";
                    street.transform.position = new Vector3(street.transform.position.x,street.transform.position.y,0.6f);
                    pavement.name = "Pavement Right";
                    pavement.transform.position = new Vector3(street.transform.position.x,street.transform.position.y,1.4f);
                break;
                case 1:
                    street.name = "Street Left";
                    street.transform.position = new Vector3(street.transform.position.x,street.transform.position.y,- 0.6f);

                    pavement.name = "Pavement Left";
                    pavement.transform.position = new Vector3(street.transform.position.x,street.transform.position.y,-1.4f);
                break;
            }
        }
    }
}
