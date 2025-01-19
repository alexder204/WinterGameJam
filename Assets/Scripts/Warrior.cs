using UnityEngine;

public class Warrior : PlayerAttack
{
    // Override the attack method for Warrior class
    public override void Attack()
    {
        // Additional warrior-specific logic (e.g., increased damage)
        currentAttackPower *= 1.5f;  // Example: Warrior does 1.5x damage

        // Optionally adjust attack speed and cooldown dynamically
        playerClassData.attackSpeed = 2f;  // This would give a faster attack speed for the Warrior

        // Recalculate attack cooldown based on new speed
        SetAttackCooldownFromSpeed();

        // Adjust movement speed specifically for the Warrior class
        playerClassData.movementSpeed = 12f;  // Set Warrior's movement speed

        Debug.Log("Warrior attacks with sword! Damage: " + currentAttackPower);

        // Call the base Attack method to trigger the animation and other functionality
        base.Attack();
    }
}
