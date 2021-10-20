using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;

    private bool b_isMoving;
    private bool b_isFacingRight;
    private bool b_isAttacking;

    public Rigidbody2D m_rigidbody;
    public Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        b_isFacingRight = true;
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
        yield return new WaitForSeconds(1.0f);
        b_isAttacking = false;
        //m_animator.SetBool("isAttacking",b_isAttacking);
        //Debug.Log("Stop attacking" + m_animator.GetBool("isAttacking"));
        m_animator.ResetTrigger("Attack");
    }
}
