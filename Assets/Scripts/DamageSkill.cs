using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Skill", menuName = "Skills/DamageSkill")]
public class DamageSkill : Skills
{
    public float damageAmount;  // Damage dealt by the skill

    // Implement the abstract Activate method
    public override void Activate(PlayerSkill playerSkill)
    {
        // Ensure the attack direction is based on the player's facing direction (via SideMovement)
        Vector2 attackDirection = playerSkill.GetComponent<SideMovement>().facingRight ? Vector2.right : Vector2.left;

        // Logic for activating the skill (e.g., attacking enemies)
        Debug.Log($"{playerSkill.name} activated {skillName} dealing {damageAmount} damage.");

        // Raycast to find enemies in the direction the player is facing
        RaycastHit2D hit = Physics2D.Raycast(playerSkill.raycastOrigin.position, attackDirection, playerSkill.currentAttackDistance, playerSkill.enemyLayer);

        // Debugging the raycast visually in the Scene view
        Debug.DrawRay(playerSkill.raycastOrigin.position, attackDirection * playerSkill.currentAttackDistance, Color.green, 0.1f);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            hit.collider.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
        else
        {
            Debug.Log("No enemy hit.");
        }
    }
}
