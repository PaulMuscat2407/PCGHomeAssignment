using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour
{
    public LSystemGeneration Lsystem;
    List<Vector3> positions = new List<Vector3>();

    private int length = 20;
    private float angle = 90;
    int counter =0;

    public int Length{
        get{
            if(length > 0){
                return length;
            }else{
                return 1;
            }
        }

        set => length = value;
    }

    public enum EncodingLetters{
        unknown = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }

    private void Start() {
        var sequence = Lsystem.GenerateSentence();
        VisualiseSequence(sequence);
    }

    private void VisualiseSequence(string sequence){
        Stack<AgentParameter> savePoints = new Stack<AgentParameter>();
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach(var letter in sequence){
            EncodingLetters encoding = (EncodingLetters)letter;

            switch(encoding){
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameter{
                        position = currentPosition,
                        direction = direction,
                        length = length
                    });
                break;
                case EncodingLetters.load:
                    if(savePoints.Count > 0){
                        var AgentParameter = savePoints.Pop();
                        currentPosition = AgentParameter.position;
                        direction = AgentParameter.direction;
                        Length = AgentParameter.length;
                    }else{
                        throw new System.Exception("No saved point in Stack");
                    }
                break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition +=direction * length;
                    CreateRoad(tempPosition,currentPosition,length,direction);
                    Length-= 2;
                    positions.Add(currentPosition);
                break;
                case EncodingLetters.turnRight:
                direction = Quaternion.AngleAxis(angle,Vector3.up) * direction;
                break;
                case EncodingLetters.turnLeft:
                direction = Quaternion.AngleAxis(-angle,Vector3.up) * direction;
                break;
            }
        }
    }

    private void CreateRoad(Vector3 tempPosition, Vector3 currentPosition,int length, Vector3 direction)
    {
        Vector3 rotationDirection = currentPosition - tempPosition;
        
        GameObject Road = new GameObject();
        Road.name = "Street "+ counter;  
        Road.AddComponent<Street>();
        Road.GetComponent<Street>().setStreetLength(length);

        if(direction == new Vector3(0,0,-1 )||direction == new Vector3(0,0,1)){
            
            Road.GetComponent<Street>().setRotation(Quaternion.Euler(0,90,0));
        }
        
        Road.GetComponent<Street>().setPos(Vector3.Lerp(tempPosition,currentPosition,0.5f));

        counter++;

    }
}
