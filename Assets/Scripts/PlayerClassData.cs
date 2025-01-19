using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerClass", menuName = "Scriptable Objects/PlayerClass")]
public class PlayerClassData : ScriptableObject
{
    public string className;  // Class name (e.g., Warrior, Archer, Mage)
    public float attackPower;  // Damage dealt by the attack
    public float attackDistance;  // Range of the attack
    public float attackSpeed;  // Speed of the attack (influences cooldown time)
    public float attackCooldownTime;  // The cooldown time for the attack
    public float movementSpeed;  // Movement speed specific to the class
    public int maxJumpCount;
    public float jumpForce;
}
