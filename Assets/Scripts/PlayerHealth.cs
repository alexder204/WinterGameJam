using UnityEngine;
using UnityEngine.UI;  // For linking UI elements like Health Bar (optional)

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Starting health
    public Slider healthSlider;  // If you're using a health bar

    void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }

    // This method reduces health when damage is applied
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (healthSlider != null)  // Update health bar if attached
        {
            healthSlider.value = health;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // This method handles player death
    void Die()
    {
        Debug.Log("Player is dead!");
        // Add death logic here (e.g. Game Over, reload scene, etc.):
        Application.Quit();  // For now, just close the game
    }

    // This method restores health after a delay when all enemies are defeated
    public void RestoreHealth(float amount)
    {
        health += amount;

        // Ensure health doesn't exceed the maximum value
        health = Mathf.Min(health, 100f);

        if (healthSlider != null)  // Update health bar if attached
        {
            healthSlider.value = health;
        }

        Debug.Log("Health Restored! Current Health: " + health);
    }
}
