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

    [Header("Attribute")] 
    public float MaxHP;
    private float HP;
    public float HitRecoverTime;
    public bool HasKey;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        AttackCounter += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isHit)
        {
            return;
        }
        
        float DisToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (DisToPlayer <= DetectRadius && DisToPlayer >= AttackRadius)
        {
            //Move to player
            rigidBody.velocity = (Player.transform.position - transform.position).normalized * MoveSpeed;
            
            //Debug.Log(Dir);
            //Vector3 move = (Player.position - transform.position).normalized * MoveSpeed * Time.deltaTime;
            //rigidBody.MovePosition(transform.position + move);
            //transform.position = Vector3.MoveTowards(transform.position, Player.position, MoveSpeed * Time.deltaTime);
        }
        else if (DisToPlayer <= AttackRadius && AttackCounter >= AttackInterval)
        {
            GetAttackDir();
            animator.SetTrigger("Attack");
            AttackCounter = 0.0f;
            rigidBody.velocity =Vector2.zero;
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

    public void DecHP(float damage, int AttackDir)
    {
        //Debug.Log("beikanle");
        HP -= damage;
        animator.SetTrigger("BeHit");
        isHit = true;
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
            if (HasKey)
            {
                Instantiate(NewKey, transform.position, Quaternion.identity);
            }

            Player.AddScore(score);
            this.gameObject.SetActive(false);
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
}
