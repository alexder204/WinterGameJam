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

    public bool facingRight = true; // Control for player's facing direction
    private float moveDirection;

    private PlayerAttack playerAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        jumpCount = maxJumpCount;
    }

    void Update()
    {
        // Handle movement inputs and jumping
        ProcessInputs();

        // Handle animations and player flip
        Animate();
    }

    private void FixedUpdate()
    {
        // Check if player is grounded and apply movement
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpCount = maxJumpCount;
        }

        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        if (isJumping && jumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount--;
            isJumping = false;
        }
    }

    private void Animate()
    {
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
        moveDirection = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);  // Flip sprite horizontally
    }
}
