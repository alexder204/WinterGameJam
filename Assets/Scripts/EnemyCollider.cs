using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public float health = 100f;  // Enemy's health
    public float damageToPlayer = 10f;  // Amount of damage the enemy does to the player

    // Movement Variables
    public float moveSpeed = 5f;
    public float followDistance = 5f;
    public float minDistance = 2f;
    private Transform player;
    private Rigidbody2D rb;

    // Facing Direction
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Movement and follow player logic
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (player.position.x > transform.position.x)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }

        if (distanceToPlayer > minDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop movement when close
        }

        FlipCharacter();
    }

    private void FlipCharacter()
    {
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // When colliding with player, deal damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);  // Deal damage to the player
            }
        }
    }

    // Handle damage to enemy and death
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy Health: " + health);  // Check the health in console for debugging

        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy enemy when health reaches zero
        }
    }
}
