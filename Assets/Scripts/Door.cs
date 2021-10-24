////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Door.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/22/2021
//Description : Class for doors
//Revision History:
//10/22/2021: Implement feature of opening door and generate portal
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    //private bool isDoorOpen=false;
    public bool isPlayerCanOpen;
    public Player player;
    public Portal portal;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Destroy when the player has the key and generate a portal in the position of the door
    void Update()
    {
        if (isPlayerCanOpen && player.getHasKey())
        {
            //isDoorOpen = true;
            Instantiate(portal, transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
