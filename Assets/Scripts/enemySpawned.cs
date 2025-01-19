using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;  // Regular enemy prefab
    public GameObject enemy1Prefab;  // Special enemy prefab
    public int enemiesToSpawn = 5;  // Number of enemies to spawn in each wave
    public Transform[] spawnPoints;  // Array of spawn points

    private List<GameObject> activeEnemies = new List<GameObject>(); // Keep track of active enemies

    public PlayerHealth playerHealth;  // Reference to the player's health script
    public float healthRestoreAmount = 20f;  // Amount of health to restore
    public float restoreDelay = 2f;  // Delay before restoring health

    void Start()
    {
        StartCoroutine(SpawnEnemiesLoop());
    }

    IEnumerator SpawnEnemiesLoop()
    {
        while (true) // Endless loop for spawning waves
        {
            SpawnEnemies(); // Spawn enemies
            yield return new WaitUntil(AllEnemiesDefeated); // Wait until all enemies are defeated

            // Restore health after all enemies are defeated with a delay
            if (playerHealth != null)
            {
                yield return new WaitForSeconds(restoreDelay);  // Wait before restoring health
                playerHealth.RestoreHealth(healthRestoreAmount);  // Restore health
            }
        }
    }

    void SpawnEnemies()
    {
        activeEnemies.Clear(); // Clear the list of active enemies

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Select a random spawn point from the array
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the regular enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().scoreValue = 10; // Assign 10 points for regular enemies

            // Instantiate the special enemy
            GameObject enemy1 = Instantiate(enemy1Prefab, spawnPoint.position, Quaternion.identity);
            enemy1.GetComponent<Enemy>().scoreValue = 15; // Assign 15 points for enemy1

            // Add them to the active enemies list
            activeEnemies.Add(enemy);
            activeEnemies.Add(enemy1);
        }
    }

    bool AllEnemiesDefeated()
    {
        // Check if all enemies in the list are null (destroyed)
        activeEnemies.RemoveAll(enemy => enemy == null); // Clean up null references
        return activeEnemies.Count == 0;  // Return true when all enemies are defeated
    }
}
