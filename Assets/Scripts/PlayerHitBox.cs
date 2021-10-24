using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("enter!"+this.name);
        int BeHitDir = 0;
        if (other.CompareTag("Enemy"))
        {
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
            
            other.GetComponent<Slime>().DecHP(Owner.GetComponent<Player>().getAttack(),BeHitDir);

            Rigidbody2D enemyRigidbody = other.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 StepBackVector = (enemyRigidbody.transform.position - transform.position).normalized * thrust;
                enemyRigidbody.AddForce(StepBackVector,ForceMode2D.Impulse);
                StartCoroutine(StepBack(enemyRigidbody));
            }
        }
    }

    private IEnumerator StepBack(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            //Debug.Log("tuihou");
            yield return new WaitForSeconds(StepBackTime);
            enemy.velocity=Vector2.zero;
        }
    }
}
