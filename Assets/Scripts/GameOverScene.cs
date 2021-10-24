////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: GameOverScene.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for GameOverScene
//Revision History:
//10/23/2021: Implement feature of changing scenes and setting level data
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    public TMP_Text WinText;
    public TMP_Text ScoreText;

    private AudioSource audioSource;
    public AudioClip WinMusic;
    public AudioClip LoseMusic;
    
    // Based on Leveldata set text content and BGM
    void Start()
    {
        bool isWin = GameObject.Find("GameController").GetComponent<LevelData>().isWin;
        if (isWin)
        {
            WinText.text = "YOU WIN";
            /*Debug.Log(WinMusic);
            audioSource.clip = WinMusic;
            audioSource.Play();*/
        }
        else
        {
            WinText.text = "GAME OVER";
            /*Debug.Log(LoseMusic);
            audioSource.clip = LoseMusic;
            audioSource.Play();*/
        }

        ScoreText.text = "Score: " + GameObject.Find("GameController").GetComponent<LevelData>().Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
