using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

    // Method to handle damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);  // Destroy the enemy when health reaches 0
        }
    }
}
