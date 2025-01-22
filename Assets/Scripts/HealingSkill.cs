using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Skill", menuName = "Skills/HealingSkill")]
public class HealingSkill : Skills
{
    public float healingAmount;  // Amount of health restored

    public override void Activate(PlayerSkill playerSkill)
    {
        // Logic for activating the skill (e.g., healing the player)
        Debug.Log($"{playerSkill.name} activated {skillName} restoring {healingAmount} health.");

        playerSkill.GetComponent<PlayerHealth>().RestoreHealth(healingAmount);
    }
}
