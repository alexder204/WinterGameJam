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

    private Rigidbody2D rb;

    public bool facingRight = true;
    private float moveDirection;

    private PlayerAttack playerAttack;
    private Animator animator;

    private float currentMoveSpeed;
    private bool canMove = true;
    public float speedRecoveryTime = 2f;
    private float speedRecoveryRate;

    [Header("Player Class Data")]
    public PlayerClassData playerClassData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();

        // Initialize movement speed, maxJumpCount, and jumpForce based on class
        if (playerClassData != null)
        {
            moveSpeed = playerClassData.movementSpeed;
            maxJumpCount = playerClassData.maxJumpCount;
            jumpForce = playerClassData.jumpForce;
        }

        currentMoveSpeed = moveSpeed;
        speedRecoveryRate = moveSpeed / speedRecoveryTime;
    }

    void Start()
    {
        jumpCount = maxJumpCount;
    }

    void Update()
    {
        // Handle movement inputs and jumping if allowed
        if (canMove)
        {
            ProcessInputs();
        }

        // Handle animations and player flip
        Animate();
    }

    private void FixedUpdate()
    {
        // Check if player is grounded and apply movement
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpCount = maxJumpCount;  // Reset jump count when grounded
        }

        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection * currentMoveSpeed, rb.linearVelocity.y);

        if (isJumping && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount--;
            isJumping = false;
        }
    }

    private void Animate()
    {
        if (moveDirection != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }

        // Play jumpup or jumpdown animations based on vertical velocity
        if (rb.linearVelocity.y > 0f)  // Player is going up
        {
            animator.SetBool("isJumpingUp", true);
            animator.SetBool("isJumpingDown", false);
        }
        else if (rb.linearVelocity.y < 0f)  // Player is going down
        {
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", true);
        }
        else if (isGrounded)  // Player is on the ground
        {
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingDown", false);
        }
    }

    private void ProcessInputs()
    {
        moveDirection = Input.GetAxis("Horizontal");

        // Check for jump input
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;

            // Trigger the "jump pressed" animation and log for debugging
            Debug.Log("Jump button pressed!");
            animator.SetBool("isJumpPressed", true);
        }

        // Stop the "jump pressed" animation once the player leaves the ground
        if (isJumping && jumpCount > 0)
        {
            animator.SetBool("isJumpPressed", false);
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
