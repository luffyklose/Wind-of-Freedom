using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    public TMP_Text WinText;
    public TMP_Text ScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        bool isWin = GameObject.Find("LevelDate").GetComponent<LevelData>().isWin;
        if (isWin)
        {
            WinText.text = "YOU WIN";
        }
        else
        {
            WinText.text = "GAME OVER";
        }

        ScoreText.text = "Score: " + GameObject.Find("LevelDate").GetComponent<LevelData>().Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
