////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Slime.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for Slime
//Revision History:
//10/19/2021:
//Set animation parameters;
//10/22/2021:
//Add basic AI features;
//Draw Gizmo;
//Update data and slime's state;
//10/24/2021:
//Add sound effects;
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public Player Player;
    public GameObject NewKey;
    public float DetectRadius;
    public float AttackRadius;
    public float MoveSpeed;
    public float AttackInterval;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private float AttackCounter = 0.0f;
    private bool isFacingRight = true;
    private bool isHit = false;
    private bool isActive = true;

    [Header("Attribute")] 
    public float MaxHP;
    private float HP;
    public float HitRecoverTime;
    public bool HasKey;
    public int score;

    [Header("Audio")] 
    private AudioSource audioSource;
    public AudioClip AttackFX;
    public AudioClip BeHitFX;
    public AudioClip DieFX;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //Update attack counter
        if (!isActive)
            return;
        AttackCounter += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //Do nothing if Slime is hit or died
        if (isHit || !isActive)
        {
            return;
        }
        
        //Move to player when the distance between player and slime is smaller than detect radius.
        //Attack player when the distanced is smaller than attack radius
        //Keep idle when the distance is bigger than detect radius
        float DisToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (DisToPlayer <= DetectRadius && DisToPlayer >= AttackRadius)
        {
            //Move to player
            rigidBody.velocity = (Player.transform.position - transform.position).normalized * MoveSpeed;
        }
        else if (DisToPlayer <= AttackRadius && AttackCounter >= AttackInterval)
        {
            GetAttackDir();
            animator.SetTrigger("Attack");
            AttackCounter = 0.0f;
            rigidBody.velocity =Vector2.zero;
            audioSource.PlayOneShot(AttackFX);
        }
        else if (DisToPlayer >= DetectRadius)
        {
            rigidBody.velocity =Vector2.zero;
        }

        //Debug.Log(rigidBody.velocity + " " + isFacingRight);
        if (rigidBody.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving",true);
            animator.SetFloat("MoveX",rigidBody.velocity.x);
            animator.SetFloat("MoveY",rigidBody.velocity.y);
            
            //Flip if the direction isn't correct
            if (rigidBody.velocity.x > 0 && !isFacingRight)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                isFacingRight = true;
            }
            else if (rigidBody.velocity.x < 0 && isFacingRight)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                isFacingRight = false;
            }
        }
        else
        {
            animator.SetBool("isMoving",false);
        }
    }

    //Draw auxiliary lines to show the detection range and attack range
    private void OnDrawGizmos()
    {
        //draw a circle to show the detect and attack circle
        // 设置矩阵
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        // 设置颜色
        Color defaultColor = Gizmos.color;
        Gizmos.color = Color.green;

        // 绘制圆环
        Vector3 beginPoint = transform.position;
        Vector3 firstPoint = transform.position;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
        {
            float x = DetectRadius * Mathf.Cos(theta);
            float y = DetectRadius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, y, 0);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }
        
        // 设置颜色
        defaultColor = Gizmos.color;
        Gizmos.color = Color.red;

        // 绘制圆环
        beginPoint = Vector3.zero;
        firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
        {
            float x = AttackRadius * Mathf.Cos(theta);
            float y = AttackRadius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, y, 0);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }

        // 绘制最后一条线段
        Gizmos.DrawLine(firstPoint, beginPoint);

        // 恢复默认颜色
        Gizmos.color = defaultColor;

        // 恢复默认矩阵
        Gizmos.matrix = defaultMatrix;
    }

    //Get attack direction for animation
    private void GetAttackDir()
    {
        if (Player.transform.position.x < transform.position.x - 0.5f)
        {
            //set attack direction to left
            animator.SetFloat("AttackDir",1.0f);
        }
        else if (Player.transform.position.x > transform.position.x + 0.5f)
        {
            //set attack direction to right
            animator.SetFloat("AttackDir",0.25f);
        }
        else if (Player.transform.position.y > transform.position.y + 0.5f)
        {
            //set attack direction to up
            animator.SetFloat("AttackDir",0.0f);
        } 
        else if (Player.transform.position.y < transform.position.y - 0.5f)
        {
            //set attack direction to down
            animator.SetFloat("AttackDir",0.75f);
        }
    }

    //Update HP and check death state
    public void DecHP(float damage, int AttackDir)
    {
        //Debug.Log("beikanle");
        HP -= damage;
        animator.SetTrigger("BeHit");
        isHit = true;
        audioSource.PlayOneShot(BeHitFX);
        switch (AttackDir)
        {
            //1:up 2:right 3:down 4:left
            case 1:
                animator.SetFloat("BeHitDir",0.0f);
                break;
            case 2:
                animator.SetFloat("BeHitDir",0.33f);
                break;
            case 3:
                animator.SetFloat("BeHitDir",0.67f);
                break;
            case 4:
                animator.SetFloat("BeHitDir",1.0f);
                break;
            default:
                break;
        }
        if (HP <= 0.0f)
        {
            Debug.Log("yinggaisile");
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(HitRecover());
        }
    }

    private IEnumerator HitRecover()
    {
        yield return new WaitForSeconds(HitRecoverTime);
        isHit = false;
    }

    //Play death sound effect and destroy itself. Generate a key when necessary
    private IEnumerator Die()
    {
        Debug.Log("sile");
        isActive = false;
        if (HasKey)
        {
            Instantiate(NewKey, transform.position, Quaternion.identity);
        }

        Player.AddScore(score);
        audioSource.PlayOneShot(DieFX);
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }
}
