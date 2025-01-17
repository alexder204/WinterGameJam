using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerClassData playerClassData;

    private float currentAttackPower;
    private float currentAttackDistance;

    public Transform raycastOrigin;  // Single raycast origin at the center of the player

    private RaycastHit2D hit;

    private SideMovement sideMovement;  // Reference to SideMovement to access facing direction

    public LayerMask enemyLayer;

    void Start()
    {
        currentAttackPower = playerClassData.attackPower;
        currentAttackDistance = playerClassData.attackDistance;

        sideMovement = GetComponent<SideMovement>();

        // You only need one raycast, it's always active
        raycastOrigin.gameObject.SetActive(true);
    }

    void Update()
    {
        // Check for attack input
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    // Attack method (Handles both Melee and Ranged attacks)
    public virtual void Attack()
    {
        Debug.Log("Attacking with power: " + currentAttackPower);

        hit = default(RaycastHit2D);

        // Cast ray in the correct direction based on the player's facing direction
        Vector2 attackDirection = sideMovement.facingRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(raycastOrigin.position, attackDirection, currentAttackDistance, enemyLayer);

        // Debugging the raycast for visual feedback
        Debug.DrawRay(raycastOrigin.position, attackDirection * currentAttackDistance, Color.green, 0.1f);

        // Handle Raycast hit
        if (hit.collider != null)
        {
            // Check if the hit object is an enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                HandleRaycastHit(hit);
            }
            else
            {
                Debug.Log("Hit object is not an enemy: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("No enemy in range.");
        }
    }

    private void HandleRaycastHit(RaycastHit2D hit)
    {
        // Handle the raycast hit (checking if it's an enemy)
        if (hit.collider.CompareTag("Enemy"))
        {
            // Handle enemy damage here
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentAttackPower);
                Debug.Log("Enemy hit with damage: " + currentAttackPower);
            }
        }
    }
}
