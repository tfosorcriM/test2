using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool isOnGroundNormal=true;
    public float footOffset = 0.375f; 
    public float groundDistance = 3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private Image cdImage;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;

    int speedID;
    int groundID;
    int fallID;

    private void Awake()
    {
        // rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        speedID = Animator.StringToHash("speed");
        groundID = Animator.StringToHash("isOnGroundNormal");
        fallID = Animator.StringToHash("yVelocity");

        cdImage = GameObject.Find("Canvas/Icon/Dash").GetComponent<Image>();
    }

    void PhysicsCheck()
    {
        RaycastHit2D footCheck = Raycast(new Vector2(0f, 0f), Vector2.down, groundDistance, groundLayer);

        //判断角色脚底射线与地面图层发生接触
        isOnGroundNormal = true;
        if (footCheck)
        {
            isOnGroundNormal = true;
        }
        else
        {
            isOnGroundNormal = false;
        }
        isOnGroundNormal = true;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        ProcessInputs();

        //animate
        Animate();
    }

    private void FixedUpdate()
    {
        PhysicsCheck();

        //move
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        if (isJumping && isOnGroundNormal) 
        // if (isJumping)
        {
            // rb.velocity = new Vector2(5, rb.velocity.y);
            rb.AddForce(new Vector2(rb.velocity.x, 200f));
        }
        isJumping = false;
    }

    private void Animate()
    {
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            FlipCharacter();
        }
    }

    private void ProcessInputs()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")) 
        {
            isJumping = true;
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    /// <summary>
    /// 射线函数重载
    /// </summary>
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length, color);

        return hit;
    }

}
