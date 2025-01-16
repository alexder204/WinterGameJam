using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMovement : MonoBehaviour
{
    [Header("Movement values")]
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public float dashSpeed = 15f;
    [SerializeField] public float jumpForce = 8f;

    [Header("Ground/Ceiling Check")]
    public float checkRadius;
    public Transform ceilingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects;
    private bool isGrounded = false;

    [Header("Jump Settings")]
    [SerializeField] public int maxJumpCount;
    private bool isJumping = false;
    private int jumpCount;

    [Tooltip("Direction Control")]
    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        jumpCount = maxJumpCount;
    }


    void Update()
    {
        // Inputs
        ProcessInputs();

        // Animations
        Animate();
    }

    private void FixedUpdate()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        // Reset jump count when grounded and if the player was jumping
        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpCount = maxJumpCount;
        }

        // Movement
        Move();
    }

    private void Move()
    {
        // Update horizontal movement
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        // Only apply jump force when jumping is allowed and jumpCount is valid
        if (isJumping && jumpCount > 0)
        {
            // Reset vertical velocity to prevent cumulative jumps
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            // Apply the jump force
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount--;
            isJumping = false;  
        }
    }

    private void Animate()
    {
        // Flip the character
        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    private void ProcessInputs()
    {
        // Horizontal movement input
        moveDirection = Input.GetAxis("Horizontal");

        // Jump input processing 
        if (Input.GetButtonDown("Jump") && jumpCount > 0) 
        {
            isJumping = true;
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
