using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystemGeneration : MonoBehaviour
{
    public Rule[] rules;
    public string rootSentence;
    private int iterationLimit;
    private float chanceToIgnoreRule;
    private bool randomIgnoreRuleModifier;

    private void Awake() {
        randomIgnoreRuleModifier = (UnityEngine.Random.value > 0.5f);
        iterationLimit = UnityEngine.Random.Range(2,6);
        chanceToIgnoreRule =  UnityEngine.Random.Range(0.0f,1.0f);
    }
    public string GenerateSentence(string word = null){
        if(word == null){
            word = rootSentence;
        }
        return GrowRecursive(word);
    }

    private string GrowRecursive(string word,int iterationIndex = 0)
    {
        if(iterationIndex >= iterationLimit){
            return word;
        }

        StringBuilder newWord = new StringBuilder();

        foreach(var character in word){
            newWord.Append(character);
            ProcessRuleRecusively(newWord,character,iterationIndex);
        }

        return newWord.ToString();
    }

    private void ProcessRuleRecusively(StringBuilder newWord, char character, int iterationIndex)
    {
        foreach(var rule in rules){
            if(rule.letter == character.ToString()){

                if(randomIgnoreRuleModifier && iterationIndex > 1){

                    if(UnityEngine.Random.value < chanceToIgnoreRule){

                        return;

                    }
                }

                newWord.Append(GrowRecursive(rule.GetResult(),iterationIndex + 1));

            }
        }
    }
}
