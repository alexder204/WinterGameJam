using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public Skills[] skills;  // Array of skills available to the player
    public KeyCode[] skillKeys;  // Custom keybindings for each skill
    public PlayerClassData playerClassData;  // Reference to PlayerClassData ScriptableObject
    public Transform raycastOrigin;  // Origin for raycasting attacks
    public LayerMask enemyLayer;  // Enemy layer for detecting hits

    // These values now come from the PlayerClassData ScriptableObject
    public float currentAttackDistance;
    public float attackPower;

    private Animator animator;
    private SideMovement sideMovement;  // Reference to the SideMovement script

    void Start()
    {
        animator = GetComponent<Animator>();

        // Initialize attack properties from PlayerClassData
        currentAttackDistance = playerClassData.attackDistance;
        attackPower = playerClassData.attackPower;

        // Initialize skills so they are ready to use immediately
        foreach (var skill in skills)
        {
            skill.InitializeSkill();
        }

        // Get reference to the SideMovement script
        sideMovement = GetComponent<SideMovement>();
    }

    void Update()
    {
        // Check input and activate skills based on custom keybindings
        for (int i = 0; i < skills.Length; i++)
        {
            // Ensure the skill is ready to be used and check for the corresponding key press
            if (Input.GetKeyDown(skillKeys[i]) && skills[i].IsReady())
            {
                // Activate the skill and apply cooldown
                skills[i].ActivateWithCooldown(this);
            }
        }
    }

    // Method for handling attacks using raycasts based on player facing direction
    public void Attack()
    {
        // Ensure the raycast direction is based on the player's facing (via SideMovement)
        Vector2 attackDirection = sideMovement.facingRight ? Vector2.right : Vector2.left;

        // Debug the direction to check if it is being set correctly
        Debug.Log("Attack Direction: " + (sideMovement.facingRight ? "Right" : "Left"));

        // Raycast to find enemies in the direction the player is facing
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, attackDirection, currentAttackDistance, enemyLayer);

        // Debugging the raycast for visual feedback
        Debug.DrawRay(raycastOrigin.position, attackDirection * currentAttackDistance, Color.green, 0.1f);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // If we hit an enemy, apply damage (damage logic should be handled in your specific skill)
            hit.collider.GetComponent<Enemy>().TakeDamage(attackPower);
        }
        else
        {
            Debug.Log("No enemy hit.");
        }
    }
}
