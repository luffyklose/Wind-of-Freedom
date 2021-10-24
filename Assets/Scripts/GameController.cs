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

    // Update is called once per frame
    void Update()
    {
        if (player != null)
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
