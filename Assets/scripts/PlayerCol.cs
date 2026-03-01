using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class PlayerCol : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private float move;

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
    [Header("Effect")]
    public Animator camAnimator;
    bool spawnDust = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        if (facingRight == false && inputHorizontal > 0)
        {
            Flib();
        }
        else if (facingRight == false && inputHorizontal < 0)
        {
            Flib();
        }

        void Flib()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            facingRight = true;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= 1;
            transform.localScale = Scaler;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded==true)
        {
            if (spawnDust == true)
            {
                camAnimator.SetTrigger("Shake");
                spawnDust = false;
            }
            extraJump = extraJumpValue;
            if(rb.linearVelocity.y == 0)
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.linearVelocity = Vector2.up * jumpForce;
            extraJump--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJump == 0 && isGrounded == true)
        {
            isGrounded = true;
            jumpTimeCounter = jumpTime;
            rb.linearVelocity = Vector2.up * jumpForce;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = true;
            }
        }
    }
}
