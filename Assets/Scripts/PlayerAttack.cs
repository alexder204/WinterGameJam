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

    // Combo system variables
    private int comboIndex = 0;  // Tracks the current combo state
    public float comboWindowTime = 1f;  // Time window to input combo (in seconds)
    private float comboTimer = 0f;  // Timer to track how much time is left for combo input

    void Start()
    {
        animator = GetComponent<Animator>();  // Get the Animator component
        sideMovement = GetComponent<SideMovement>();  // Get the SideMovement component

        if (playerClassData != null)
        {
            currentAttackPower = playerClassData.attackPower;
            currentAttackDistance = playerClassData.attackDistance;
            currentAttackSpeed = playerClassData.attackSpeed;
        }

        raycastOrigin.gameObject.SetActive(true);
    }

    void Update()
    {
        // Decrease the cooldown timer over time
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
        }

        // Update the combo timer when combo input is active
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();  // Reset combo if the time window is over
            }
        }

        // Handle combo input (attack button)
        if (Input.GetButtonDown("Fire1") && currentCooldownTime <= 0 && !isAttacking)
        {
            PerformComboAttack();
        }
    }

    // Perform combo attack based on current comboIndex
    private void PerformComboAttack()
    {
        // Handle the first combo stage (Attack0)
        if (comboIndex == 0)
        {
            Attack();  // Perform the attack
            comboIndex = 1;  // Move to the second attack in the combo
            animator.SetInteger("ComboIndex", comboIndex);  // Update Animator to ComboIndex 1
            comboTimer = comboWindowTime;  // Reset the combo timer
        }
        // Handle the second combo stage (Attack1)
        else if (comboIndex == 1 && comboTimer > 0f)
        {
            Attack();  // Perform the attack
            comboIndex = 2;  // Move to the third attack in the combo
            animator.SetInteger("ComboIndex", comboIndex);  // Update Animator to ComboIndex 2
            comboTimer = comboWindowTime;  // Reset the combo timer
        }
        // Handle the third combo stage (Attack2)
        else if (comboIndex == 2 && comboTimer > 0f)
        {
            Attack();  // Perform the attack
            ResetCombo();  // After this attack, the combo ends
        }
    }

    // Resets the combo system when time expires or after completing a combo
    private void ResetCombo()
    {
        comboIndex = 0;  // Reset combo index to 0
        animator.SetInteger("ComboIndex", comboIndex);  // Reset ComboIndex in Animator
        comboTimer = 0f;
    }

    // Method to handle the attack
    public virtual void Attack()
    {
        // Trigger the appropriate animation (attack animations should be named "Attack0", "Attack1", "Attack2", etc.)
        animator.SetTrigger("Attack" + comboIndex);  // Trigger different animations for different combo stages

        // Start the cooldown after the attack
        currentCooldownTime = currentAttackCooldownTime;
        isAttacking = true;

        hit = default(RaycastHit2D);

        // Cast ray in the correct direction based on the player's facing direction
        Vector2 attackDirection = sideMovement.facingRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(raycastOrigin.position, attackDirection, currentAttackDistance, enemyLayer);

        // Handle Raycast hit
        if (hit.collider != null)
        {
            // Check if the hit object is an enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                HandleRaycastHit(hit);
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
            }
        }
    }

    // Reset attacking flag once the attack animation is finished
    public void FinishAttackAnimation()
    {
        isAttacking = false;
    }

    // Method to set the attack cooldown based on attack speed
    protected void SetAttackCooldownFromSpeed()
    {
        // Calculate attack cooldown based on attack speed (lower attackSpeed means faster attack)
        currentAttackCooldownTime = 1 / currentAttackSpeed;  // This assumes attackSpeed > 0
    }
}
