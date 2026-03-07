using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCol : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    private Rigidbody2D rb;
    private bool facingRight = true;

    [Header("Jump")]
    public float extraJumpValue;
    public float jumpTime;
    public float extraJump;
    private float jumpTimeCounter;
    private bool isJumping;

    [Header("Ground")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool wasGrounded;

    [Header("Effect")]
    public Animator camAnimator;
    public GameObject dustEffect;
    bool spawnDust = false;

    [Header("Animation")]
    public Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        if (Mathf.Abs(move) > 0 && isGrounded)
        {
            playerAnimator.SetBool("PlayerRun", true);
        }
        else
        {
            playerAnimator.SetBool("PlayerRun", false);
        }

        if (facingRight == false && move > 0) Flib();
        else if (facingRight == true && move < 0) Flib();
    }

    void Update()
    {
        if (isGrounded == true)
        {
            if (wasGrounded == false)
            {
                playerAnimator.SetTrigger("PlayerLand");
            }

            if (spawnDust == true)
            {
                camAnimator.SetTrigger("Shake");
                Instantiate(dustEffect, groundCheck.position, Quaternion.identity);
                spawnDust = false;
            }

            if (rb.linearVelocity.y <= 0.1f)
            {
                extraJump = extraJumpValue;
                isJumping = false;
            }
        }
        else
        {
            spawnDust = true;
        }

        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (extraJump > 0 || isGrounded == true)
            {
                Instantiate(dustEffect, groundCheck.position, Quaternion.identity);
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rb.linearVelocity = Vector2.up * jumpForce;

                playerAnimator.SetTrigger("PlayerJump");

                if (extraJump > 0) extraJump--;
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else isJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;
        if (Input.GetKeyDown(KeyCode.U)) playerAnimator.SetTrigger("PlayerDeath");
        if (Input.GetKeyDown(KeyCode.I)) playerAnimator.SetTrigger("PlayerHurt");
        if (Input.GetKeyDown(KeyCode.O)) playerAnimator.SetTrigger("PlayerSpawn");
        if (Input.GetKeyDown(KeyCode.P)) playerAnimator.SetTrigger("PlayerVictory");
    }

    void Flib()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}