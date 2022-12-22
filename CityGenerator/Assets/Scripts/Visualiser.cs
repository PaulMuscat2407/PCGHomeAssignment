using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour
{

    public GameObject Player;
    public LSystemGeneration Lsystem;
    List<Vector3> positions = new List<Vector3>();

    private int length = 20;
    private float angle = 90;
    int counter = 0;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }

        set => length = value;
    }

    public enum EncodingLetters
    {
        unknown = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }

    private void Start()
    {
        //Generates a sentence using L-System to pass through the Generation funtion
        var sequence = Lsystem.GenerateSentence();
        VisualiseSequence(sequence);
    }


    private void VisualiseSequence(string sequence)
    {
        Stack<AgentParameter> savePoints = new Stack<AgentParameter>();
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        //Analyzes sentence passed, (letter by letter/symbol by symbol) and execute code in the switch statement according to the EncodingLetters Enum;
        foreach (var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;

            switch (encoding)
            {
                //Add point to savePoint stack
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameter
                    {
                        position = currentPosition,
                        direction = direction,
                        length = length
                    });
                    break;
                //Remove point from savePOint stack and set Position,direcvtion and length to popped value
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var AgentParameter = savePoints.Pop();
                        currentPosition = AgentParameter.position;
                        direction = AgentParameter.direction;
                        Length = AgentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("No saved point in Stack");
                    }
                    break;
                //Create roads and houses at points and add position to Positions list
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    CreateRoad(tempPosition, currentPosition, length, direction);
                    CreateHouses(tempPosition, currentPosition, direction);
                    Length -= 2;
                    positions.Add(currentPosition);
                    break;
                //Rotate direction Right
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                //Rotate direction Left
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
            }
        }

        //Create Spawnpoint at a random point from the Position Lists on the road and move player to that location
        Vector3 SpawnPos = positions[UnityEngine.Random.Range(0,positions.Count -2)];
        SpawnPos.y = 1.5f;

        Player.transform.position = SpawnPos;
        
        //Create Endpoint at the end point on the road and allow player to collide to go to next level
        GameObject End = new GameObject();
        End.name = "End";
        End.AddComponent<Cube>();
        End.AddComponent<End>();
        End.AddComponent<BoxCollider>();

        End.GetComponent<Cube>().setCubeSize(0.25f,15,0.25f);
        End.GetComponent<Cube>().setSubmeshIndex(4);
        End.GetComponent<BoxCollider>().size = new Vector3(0.5f,30f,0.5f);
        End.GetComponent<BoxCollider>().isTrigger = true;

        Vector3 EndPos = positions[positions.Count -1];
        EndPos.y = 15;
        End.transform.position = EndPos;
    }

    //Creates Houses
    private void CreateHouses(Vector3 tempPosition, Vector3 currentPosition, Vector3 direction)
    {
        for( int i = 1 ; i < 7; i++){
            GameObject House = new GameObject();
            House.name = "House";
            House.tag = "House";
            House.AddComponent<House>();
            House.AddComponent<BoxCollider>();

            //Randomizes House Sizes
            House.GetComponent<House>().setHouseSize(1,UnityEngine.Random.Range(2,5),1);
            House.GetComponent<BoxCollider>().size= House.GetComponent<House>().returnHouseSize();

            //Check Streets direction, and places houses accordingly on the X or Z axis.
            if (direction == new Vector3(0, 0, -1))
            {
                House.GetComponent<House>().setPos(new Vector3(House.GetComponent<House>().GetSide(tempPosition.x), House.GetComponent<House>().GetHeight(),tempPosition.z -(Vector3.Distance(tempPosition,currentPosition)* i/7)));
            }else if(direction == new Vector3(0, 0, 1)){
                House.GetComponent<House>().setPos(new Vector3(House.GetComponent<House>().GetSide(tempPosition.x), House.GetComponent<House>().GetHeight(),currentPosition.z -(Vector3.Distance(tempPosition,currentPosition)* i/7)));
            }
            else if (direction == -Vector3.right){
                House.GetComponent<House>().setPos(new Vector3(tempPosition.x -(Vector3.Distance(tempPosition,currentPosition)* i/7), House.GetComponent<House>().GetHeight(), House.GetComponent<House>().GetSide(tempPosition.z)));
            }else
            {
                House.GetComponent<House>().setPos(new Vector3(currentPosition.x -(Vector3.Distance(tempPosition,currentPosition)* i/7), House.GetComponent<House>().GetHeight(), House.GetComponent<House>().GetSide(tempPosition.z)));
            }
            
        }
        
    }

    //Creates road
    private void CreateRoad(Vector3 tempPosition, Vector3 currentPosition, int length, Vector3 direction)
    {
        GameObject Road = new GameObject();
        Road.name = "Street " + counter;
        Road.tag = "Road";
        Road.AddComponent<Street>();
        Road.AddComponent<BoxCollider>();
        //Sizes the roads
        Road.GetComponent<Street>().setStreetLength(length);
        Road.GetComponent<BoxCollider>().size = new Vector3(length,0.05f,5f);

        //Rotates street if street should be facing forward (N-S).
        if (direction == new Vector3(0, 0, -1) || direction == new Vector3(0, 0, 1))
        {

            Road.GetComponent<Street>().setRotation(Quaternion.Euler(0, 90, 0));
        }

        //Set the Road to the midpoint between tempPosition and EndPosition
        Road.GetComponent<Street>().setPos(Vector3.Lerp(tempPosition, currentPosition, 0.5f));

        counter++;

    }
}
