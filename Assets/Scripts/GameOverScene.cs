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
    
    // Start is called before the first frame update
    void Start()
    {
        bool isWin = GameObject.Find("GameController").GetComponent<LevelData>().isWin;
        if (isWin)
        {
            WinText.text = "YOU WIN";
            //audioSource.clip = WinMusic;
        }
        else
        {
            WinText.text = "GAME OVER";
            //audioSource.clip = LoseMusic;
        }

        ScoreText.text = "Score: " + GameObject.Find("GameController").GetComponent<LevelData>().Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
