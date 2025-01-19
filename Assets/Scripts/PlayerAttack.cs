using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerClassData playerClassData;  // Reference to the PlayerClassData ScriptableObject

    public float currentAttackPower;
    public float currentAttackDistance;
    public float currentAttackCooldownTime;  // Variable to hold the cooldown time
    public float currentAttackSpeed;  // Variable to store the attack speed

    public Transform raycastOrigin;  // Single raycast origin at the center of the player

    private RaycastHit2D hit;

    private SideMovement sideMovement;  // Reference to SideMovement to access facing direction

    public LayerMask enemyLayer;

    private Animator animator;  // Reference to the Animator component

    private bool isAttacking = false;  // Flag to track if the player is attacking
    private float currentCooldownTime = 0f;  // Timer to track remaining cooldown

    void Start()
    {
        // Set the attack properties from PlayerClassData
        currentAttackPower = playerClassData.attackPower;
        currentAttackDistance = playerClassData.attackDistance;

        // Set attack speed from PlayerClassData
        currentAttackSpeed = playerClassData.attackSpeed;

        // Set attack cooldown based on attack speed
        SetAttackCooldownFromSpeed();

        sideMovement = GetComponent<SideMovement>();

        raycastOrigin.gameObject.SetActive(true);

        // Initialize the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Decrease the cooldown timer over time
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
        }

        // Check for attack input and if cooldown has finished, also ensure the player isn't already attacking
        if (Input.GetButtonDown("Fire1") && currentCooldownTime <= 0 && !isAttacking)
        {
            Attack();
        }
    }

    // Attack method (Handles both Melee and Ranged attacks)
    public virtual void Attack()
    {
        // Set the attacking flag to true to prevent multiple attacks
        isAttacking = true;

        // Immediately trigger the attack animation
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);  // Trigger the "Attack" animation with a bool parameter
        }

        // Start the cooldown timer based on the player's class
        currentCooldownTime = currentAttackCooldownTime;

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

    // Reset attacking flag once the attack animation is finished
    public void FinishAttackAnimation()
    {
        // End attack animation
        isAttacking = false;
        animator.SetBool("isAttacking", false);  // Set isAttacking to false to stop the attack animation
    }

    // Method to set the attack cooldown based on attack speed
    protected void SetAttackCooldownFromSpeed()
    {
        // Calculate attack cooldown based on attack speed (lower attackSpeed means faster attack)
        currentAttackCooldownTime = 1 / currentAttackSpeed;  // This assumes attackSpeed > 0
    }
}
