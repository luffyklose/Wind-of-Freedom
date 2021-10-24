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
}
