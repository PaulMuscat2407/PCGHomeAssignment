using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour
{
    public LSystemGeneration Lsystem;

    List<Vector3> positions = new List<Vector3>();
    public RoadHelper roadHelper;

    private int length = 20;
    private float angle = 90;

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
                    currentPosition +=direction * (length*2);
                    roadHelper.PlaceStreetPositions(tempPosition,Vector3Int.RoundToInt(direction),length);
                    Length-= 2;
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
}
