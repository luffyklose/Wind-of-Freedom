using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float m_moveSpeed;

    private bool b_isMoving;
    private bool b_isFacingRight;
    private bool b_isAttacking;
    private bool b_hasKey;
    private int m_score = 0;

    public Rigidbody2D m_rigidbody;
    public Animator m_animator;
    public SpriteRenderer spriteRenderer;
    public Color color;

    [Header("Attribute")] 
    public int MaxHP;
    private int HP;
    public float AttackData;

    [Header("UI")] 
    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        
        b_isFacingRight = true;

        HP = MaxHP;

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < MaxHP)
            {
                Hearts[i].enabled = true;
            }
            else
            {
                Hearts[i].enabled = false;
            }
        }
        
        ScoreText.text = "Score: " + m_score.ToString();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !b_isAttacking)
        {
            //Debug.Log("Attack");
            StartCoroutine(Attack());
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
       Move();
       
       m_animator.SetBool("isMoving",b_isMoving);
       //Debug.Log(m_animator.GetFloat("moveX") + " " + m_animator.GetFloat("moveY"));
    }

    private void Flip()
    {
        b_isFacingRight = !b_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Move()
    {
        //Basic Movement Scripts
        m_rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_moveSpeed;

        if (m_rigidbody.velocity.magnitude > Double.Epsilon)
        {
            if (Input.GetAxisRaw ("Horizontal") > 0.5f && !b_isFacingRight) 
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                b_isFacingRight = true;
            } else if (Input.GetAxisRaw("Horizontal") < 0.5f && b_isFacingRight) 
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                b_isFacingRight = false;
            }

            b_isMoving = true;
            m_animator.SetFloat("MoveX",m_rigidbody.velocity.x);
            m_animator.SetFloat("MoveY",m_rigidbody.velocity.y);
        }
        else
        {
            b_isMoving = false;
        }
    }

    private IEnumerator Attack()
    {
        b_isAttacking = true;
        //m_animator.SetBool("isAttacking",b_isAttacking);
        //Debug.Log("Start attacking" + m_animator.GetBool("isAttacking"));
        m_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        b_isAttacking = false;
        //m_animator.SetBool("isAttacking",b_isAttacking);
        //Debug.Log("Stop attacking" + m_animator.GetBool("isAttacking"));
        m_animator.ResetTrigger("Attack");
    }

    public void GetHurt(int AttackDir)
    {
        //Debug.Log("aaaaa");
        m_animator.SetTrigger("BeHit");
        switch (AttackDir)
        {
            //1:up 2:right 3:down 4:left
            case 1:
                m_animator.SetFloat("BeHitDir",0.0f);
                break;
            case 2:
                m_animator.SetFloat("BeHitDir",0.33f);
                break;
            case 3:
                m_animator.SetFloat("BeHitDir",0.67f);
                break;
            case 4:
                m_animator.SetFloat("BeHitDir",1.0f);
                break;
            default:
                break;
        }

        HP--;
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < HP)
            {
                Hearts[i].sprite = FullHeart;
            }
            else
            {
                Hearts[i].sprite = EmptyHeart;
            }
        }

        if (HP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        color.a = 0.5f;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.5f);
        color.a = 0.25f;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    public float getAttack()
    {
        return AttackData;
    }

    public void setHasKey(bool haskey)
    {
        b_hasKey = haskey;
    }

    public bool getHasKey()
    {
        return b_hasKey;
    }

    public void AddScore(int score)
    {
        m_score += score;
        ScoreText.text = "Score: " + m_score.ToString();
        GameObject.Find("GameController").GetComponent<LevelData>().Score = m_score;
    }

    public int getScore()
    {
        return m_score;
    }
}
