////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: EnemyHitBox.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for hit boxes of enemy
//Revision History:
//10/23/2021: Implement feature of checking collision with players
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public GameObject Owner;
    public float thrust;
    public float StepBackTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Check collision of player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Get the direction where attack comes for the animation
            int BeHitDir = 0;
            switch (this.name)
            {
                case "Up":
                    BeHitDir = 3;
                    break;
                case "Down":
                    BeHitDir = 1;
                    break;
                case "Side":
                    if (other.transform.position.x > transform.position.x)
                    {
                        BeHitDir = 4;
                    }
                    else
                    {
                        BeHitDir = 2;
                    }
                    break;
                default:
                    break;
            }
            other.GetComponent<Player>().GetHurt(BeHitDir);

            //Implement knockback effects
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                Vector2 StepBackVector = (playerRigidbody.transform.position - transform.position).normalized * thrust;
                //Debug.Log("tuihou" + StepBackVector.magnitude);
                playerRigidbody.AddForce(StepBackVector,ForceMode2D.Impulse);
                StartCoroutine(StepBack(playerRigidbody));
            }
        }
    }

    //Knockback for a certain time
    private IEnumerator StepBack(Rigidbody2D player)
    {
        if (player != null)
        {
            yield return new WaitForSeconds(StepBackTime);
            player.velocity=Vector2.zero;
        }
    }
}
