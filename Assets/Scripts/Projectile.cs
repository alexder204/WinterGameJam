using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;  // Speed of the projectile
    public float lifetime = 5f;  // Time before the projectile is destroyed
    private Vector2 direction;

    private void Start()
    {
        // Destroy the projectile after a certain time
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 _direction)
    {
        // Set the direction of the projectile (left or right)
        direction = _direction;
    }

    private void Update()
    {
        // Move the projectile along the X-axis at the specified speed
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the projectile when it collides with anything
        Destroy(gameObject);

        // Check if the projectile hits the player and apply damage
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply damage to the player using the PlayerHealth script
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);  // Deal 10 damage (can be adjusted)
            }
        }
    }
}
