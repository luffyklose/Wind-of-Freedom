////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: DetectBox.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/22/2021
//Description : Class for detectbox, using to check the collision of owner and other objects
//Revision History:
//10/22/2021: Add basic function. Check collision with player
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectBox : MonoBehaviour
{
    public GameObject Owner;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Check collision with player when it's attached to a door
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Door door = Owner.GetComponent<Door>();
            if (door != null)
            {
                door.isPlayerCanOpen = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Door door = Owner.GetComponent<Door>();
            if (door != null)
            {
                door.isPlayerCanOpen = false;
            }
        }
    }
}
