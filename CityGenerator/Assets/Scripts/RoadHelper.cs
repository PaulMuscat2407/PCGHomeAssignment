using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    Dictionary<Vector3Int,GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    public void PlaceStreetPositions(Vector3 startPosition,Vector3Int direction,int length){
        var rotation = Quaternion.identity;
        if(direction.x == 0){
            rotation = Quaternion.Euler(0,90,0);
        }

        for(int i = 0 ; i< length; i++){
            var position = Vector3Int.RoundToInt(startPosition+direction*i);

            if(roadDictionary.ContainsKey(position)){
                continue;
            }
            GameObject Road = new GameObject();
            Road.transform.parent = this.transform;
            Road.AddComponent<Street>();
            Road.GetComponent<Street>().setStreetLength(length);
            Road.GetComponent<Street>().setPos(position);
            Road.GetComponent<Street>().setRotation(rotation);

            roadDictionary.Add(position,Road);
        }
    }
}
