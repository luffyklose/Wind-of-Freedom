////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Portal.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for portal
//Revision History:
//10/23/2021: Implement feature of changing scene
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("GameController").GetComponent<LevelData>().isWin = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }
}
