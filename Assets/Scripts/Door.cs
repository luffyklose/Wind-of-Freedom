using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isDoorOpen=false;
    public bool isPlayerCanOpen;
    public Player player;
    public Portal portal;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && isPlayerCanOpen && player.getHasKey())
        {
            isDoorOpen = true;
            Instantiate(portal, transform.position, Quaternion.identity);
            this.gameObject.SetActive(false);
        }
    }
}
