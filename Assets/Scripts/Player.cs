////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: Player.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 10/23/2021
//Description : Class for Player
//Revision History:
//9/30/2021:
//Implement basic movement
//10/20/2021:
//Implement basic attack;
//Set basic data;
//Implement heart and score UI display;
//Implement hurt and death;
//10/23/2021:
//Add sound effect;
//Implement touch control
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float m_moveSpeed;

    private bool b_isMoving;
    private bool b_isFacingRight;
    private bool b_isAttacking;
    private bool b_hasKey;
    public bool b_isAlive;
    private int m_score = 0;

    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private SpriteRenderer spriteRenderer;
    private Color color;
    private PlayerInput playerInput;

    [Header("Attribute")] 
    public int MaxHP;
    private int HP;
    public float AttackData;

    [Header("UI")] 
    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public Text ScoreText;

    [Header("Audio")] 
    private AudioSource audiosource;
    public AudioClip AttackFX;
    public AudioClip BeHitFX;
    public AudioClip DieFX;
    public AudioClip PickUpFX;
    

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        audiosource = GetComponent<AudioSource>();
        playerInput = GetComponent<PlayerInput>();
        
        b_isFacingRight = true;
        b_isAlive = true;

        HP = MaxHP;
        
        //add hearts based on player's HP
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
        //Get attack input
        if (playerInput.actions["Attack"].triggered && !b_isAttacking)
        {
            //Debug.Log("Attack");
            StartCoroutine(Attack());
        }
        
        //test code for player's death
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(Die());
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
       Move();
       
       m_animator.SetBool("isMoving",b_isMoving);
       //Debug.Log(m_animator.GetFloat("moveX") + " " + m_animator.GetFloat("moveY"));
    }

    //Change the face diretion
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
        //m_rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        m_rigidbody.velocity = input;
        m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_moveSpeed;

        if (m_rigidbody.velocity.magnitude > Double.Epsilon)
        {
            if (input.x > 0.5f && !b_isFacingRight) 
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                b_isFacingRight = true;
            } else if (input.x < 0.5f && b_isFacingRight) 
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

    //Play attack animations
    private IEnumerator Attack()
    {
        b_isAttacking = true;
        m_animator.SetTrigger("Attack");
        audiosource.PlayOneShot(AttackFX);
        yield return new WaitForSeconds(0.3f);
        b_isAttacking = false;
        m_animator.ResetTrigger("Attack");
    }

    //Play hurt animations.
    //Check if the player is died.
    public void GetHurt(int AttackDir)
    {
        m_animator.SetTrigger("BeHit");
        audiosource.PlayOneShot(BeHitFX);
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

    //sprite becomes translucent for a second and destroy.
    private IEnumerator Die()
    {
        audiosource.PlayOneShot(DieFX);
        color.a = 0.5f;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.5f);
        color.a = 0.25f;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.5f);
        b_isAlive = false;
        this.gameObject.SetActive(false);
    }

    public float getAttack()
    {
        return AttackData;
    }

    public void setHasKey(bool haskey)
    {
        b_hasKey = haskey;
        if (haskey)
        {
            audiosource.PlayOneShot(PickUpFX);
        }
    }

    public bool getHasKey()
    {
        return b_hasKey;
    }

    //Update score and pass the value to level data
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
