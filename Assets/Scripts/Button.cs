////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Button.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 9/30/2021
//Description : Class for buttons
//Revision History:
//9/30/2021: Implement basic load scene feature
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Jump to scene based on scene name
    public void GoToScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
