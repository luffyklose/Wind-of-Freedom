////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: GameController.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for GameController
//Revision History:
//10/23/2021: Implement feature of changing scenes and setting level data
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Player player;
    public bool isWin = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Load GameoverScene when player is died in GameplayScene. Store level data in LevelData.
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameplayScene")
        {
            if (!player.b_isAlive)
            {
                GameObject.Find("GameController").GetComponent<LevelData>().isWin = false;
                GameObject.Find("GameController").GetComponent<LevelData>().Score = player.getScore();
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
            }
        }
    }
}
