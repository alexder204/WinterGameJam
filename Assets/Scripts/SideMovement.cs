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
    public LayerMask groundLayer;  // Ground layer mask
    public LayerMask platformLayer;  // Platform layer mask
    private bool isGrounded = false;

    [Header("Jump Settings")]
    [SerializeField] public int maxJumpCount;
    public bool isJumping = false;
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

    private Collider2D playerCollider;
    private bool isFallingThrough = false;
    // Expose collider disable time so that it can be changed in Unity Inspector or via manager script
    [Header("Platform Fall Settings")]
    public float colliderDisableTime = 0.2f; // Time for the collider to stay off (public for easy access)
    private float colliderDisableTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

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

        // Manage collider disable time
        if (isFallingThrough && colliderDisableTimer > 0)
        {
            colliderDisableTimer -= Time.deltaTime;
        }
        else if (colliderDisableTimer <= 0)
        {
            // Re-enable the collider after the delay
            if (!playerCollider.enabled)
            {
                playerCollider.enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // Check if player is grounded (including platform check)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer | platformLayer);

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpCount = maxJumpCount;  // Reset jump count when grounded
        }

        // Call the method to check if the player can pass through the platform
        CheckPlatformPassThrough();

        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection * currentMoveSpeed, rb.linearVelocity.y);  // Set only horizontal velocity

        if (isJumping && jumpCount > 0)
        {
            // Apply a vertical force for the jump, using jumpForce
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);  // Cancel any vertical velocity before jumping
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);  // Apply the jump force
            jumpCount--;
            isJumping = false;
        }
    }

    private void Animate()
    {
        // Check the horizontal movement and update the animation
        if (moveDirection != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Handle flip of the character based on movement direction
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
        }

        // Check for "S" input to allow falling through platform, only if grounded
        if (Input.GetKey(KeyCode.S) && IsOnPlatform() && isGrounded && IsInIdleOrRunningState())
        {
            // Disable collider temporarily to fall through platform
            if (!isFallingThrough && playerCollider.enabled)
            {
                playerCollider.enabled = false;
                isFallingThrough = true;
                colliderDisableTimer = colliderDisableTime;
            }
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // Check if the player can pass through platforms when jumping or holding "S"
    private void CheckPlatformPassThrough()
    {
        // Re-enable the collider after some time if "S" was held
        if (isFallingThrough && colliderDisableTimer <= 0)
        {
            isFallingThrough = false;
        }

        // If the player is jumping upwards (not grounded), ignore platform collisions to allow jumping through
        if (rb.linearVelocity.y > 0f)
        {
            playerCollider.enabled = false;  // Disable collider while jumping up
        }
        else if (rb.linearVelocity.y <= 0f && !isFallingThrough)
        {
            // Re-enable collisions when falling or grounded
            playerCollider.enabled = true;
        }
    }

    // Checks if the player is currently standing on a platform
    private bool IsOnPlatform()
    {
        // You can use a check like this to make sure the player is on a platform layer
        Collider2D platformCollider = Physics2D.OverlapCircle(groundCheck.position, checkRadius, platformLayer);
        return platformCollider != null;
    }

    // Check if the player is in either the "idle" or "running" state
    private bool IsInIdleOrRunningState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the player is either in the "Idle" or "Running" state (adjust these state names based on your animation setup)
        return stateInfo.IsName("Idle") || stateInfo.IsName("Running");
    }
}
