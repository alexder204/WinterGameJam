public class Warrior : PlayerAttack
{
    private void Start()
    {
        if (playerClassData == null)
        {
            playerClassData = GetComponent<PlayerClassData>();  // or fetch it in another way if needed
        }

        // Apply class data values
        currentAttackPower = playerClassData.attackPower;
        currentAttackDistance = playerClassData.attackDistance;
        currentAttackCooldownTime = playerClassData.attackCooldownTime;
        currentAttackSpeed = playerClassData.attackSpeed;
    }

    public override void Attack()
    {
        // Warrior-specific logic before the attack
        currentAttackPower *= 1.5f;  // Warrior does 1.5x damage

        base.Attack();  // Call the base method for attack handling
    }
}
