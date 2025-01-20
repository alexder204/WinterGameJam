using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

    // Movement
    public float moveSpeed = 5f; // Enemy's fixed movement speed
    public float followDistance = 5f;
    public float minDistance = 2f;
    private Transform player;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    // Shooting
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public Transform shootPoint;  // Where the projectile will spawn
    public float shootCooldown = 2f;  // Time between shots
    private float shootTimer = 0f;  // Timer to track cooldown

    // Facing direction (for shooting)
    private bool facingRight = true;

    // Score settings
    public int scoreValue = 10;  // Points to give when defeated
    private ScoreManager scoreManager;  // Reference to ScoreManager

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Find player by tag

        // Replace the deprecated method with the new one
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();  // Get reference to the ScoreManager
    }


    private void Update()
    {
        // Check if the enemy is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Move towards the player and handle shooting
        FollowPlayer();

        // Handle shooting with cooldown
        Shoot();
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Update facing direction and flip the enemy accordingly
        if (player.position.x > transform.position.x)
        {
            facingRight = true;  // Player is to the right, so face right
        }
        else
        {
            facingRight = false;  // Player is to the left, so face left
        }

        // Only move if the distance is greater than the minimum distance
        if (distanceToPlayer > minDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Use a fixed speed for enemy movement
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);  // Keep the vertical speed the same
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);  // Stop horizontal movement if too close
        }

        // Flip the enemy to face the player
        FlipCharacter();
    }

    private void Shoot()
    {
        // Decrease shoot timer by the time passed
        shootTimer -= Time.deltaTime;

        // If cooldown has passed, shoot again
        if (shootTimer <= 0f)
        {
            // Instantiate the projectile
            ShootProjectile();

            // Reset the cooldown timer
            shootTimer = shootCooldown;
        }
    }

    private void ShootProjectile()
    {
        // Create the projectile at the shoot point
        if (projectilePrefab != null && shootPoint != null)
        {
            // Instantiate the projectile with no rotation (we will rotate it based on direction later)
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            // Ensure the projectile is enabled
            projectile.SetActive(true);

            // Get the direction towards the player (this will be the direction the projectile moves)
            Vector2 direction = facingRight ? Vector2.right : Vector2.left;  // Right if facing right, left if facing left

            // Set the direction of the projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(direction);
            }
        }
    }

    private void FlipCharacter()
    {
        // Flip the enemy sprite depending on the `facingRight` flag
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);  // 180 degrees on the Y-axis (facing right)
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);  // 0 degrees (facing left)
        }
    }

    // Handle collision with the player (taking damage when collided)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player when they collide with the enemy
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);  // 10f is the damage value
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Update the score when this enemy is destroyed
            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreValue);  // Add points to the score
            }

            Destroy(gameObject);  // Destroy the enemy when health reaches 0
        }
    }
}
