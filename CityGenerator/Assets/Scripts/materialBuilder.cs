using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialBuilder
{
    private List<Material> materialsList = new List<Material>();

    public materialBuilder(){

        Material blackMaterial = new Material(Shader.Find("Specular"));
        blackMaterial.color = Color.black;
        Material whiteMaterial = new Material(Shader.Find("Specular"));
        whiteMaterial.color = Color.white;
        Material greyMaterial = new Material(Shader.Find("Specular"));
        greyMaterial.color = Color.grey;


        materialsList.Add(blackMaterial);
        materialsList.Add(whiteMaterial);
        materialsList.Add(greyMaterial);

    }

    public List<Material> MaterialsList(){
        return materialsList;
    }

}
