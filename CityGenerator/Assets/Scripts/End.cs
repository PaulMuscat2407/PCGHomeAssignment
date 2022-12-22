using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }else{
                Debug.Log("End");
            }
            
        }
    }
}
