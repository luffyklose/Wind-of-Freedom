////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Key.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for Key
//Revision History:
//10/23/2021: Change haskey value when collided with player
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip PickUpFX;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().setHasKey(true);
            audioSource.PlayOneShot(PickUpFX);
            this.gameObject.SetActive(false);
        }
    }
}
