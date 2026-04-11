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

    public Transform bodyImg;

    // --- ส่วนที่เพิ่มเข้ามาสำหรับบันได ---
    [Header("Ladder")]
    public float climbSpeed = 5f;
    private bool isAtLadder;
    private bool isClimbing;
    private float defaultGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // --- เก็บค่า Gravity เริ่มต้นของตัวละครไว้ ---
        defaultGravity = rb.gravityScale;
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

        // --- ระบบฟิสิกส์ตอนปีนบันได ---
        if (isClimbing)
        {
            float verticalMove = Input.GetAxis("Vertical");
            rb.gravityScale = 0f; // ปิดแรงโน้มถ่วงไม่ให้ร่วง
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalMove * climbSpeed); // บังคับขึ้นลง
        }
        else
        {
            rb.gravityScale = defaultGravity; // ถ้าไม่ได้ปีน คืนค่าแรงโน้มถ่วงปกติ
        }
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
        // --- เช็คว่ากดปุ่มขึ้น/ลง ตรงบันไดหรือเปล่า ---
        float verticalInput = Input.GetAxis("Vertical");
        if (isAtLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isClimbing = true;
            isJumping = false; // ยกเลิกการกระโดดตอนเริ่มปีน
        }

        // เพิ่มเงื่อนไข !isClimbing เข้าไป เพื่อไม่ให้กดกระโดดได้ตอนกำลังเกาะบันไดอยู่
        if (Input.GetKeyDown(KeyCode.Space) && !isClimbing)
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
        Vector3 Scaler = bodyImg.localScale;
        Scaler.x *= -1;
        bodyImg.localScale = Scaler;
    }

    // --- ตรวจจับว่าเดินชนบันได (Trigger) หรือยัง ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isAtLadder = true;
        }
    }

    // --- ตรวจจับว่าเดินออกจากบันไดหรือยัง ---
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isAtLadder = false;
            isClimbing = false;
        }
    }
}