using UnityEngine;

public class Warrior : PlayerAttack
{
    // Override the attack method
    public override void Attack()
    {
        Debug.Log("Warrior attacks with a sword!");
        base.Attack();  // Optionally, call the base Attack method if you want to retain the default functionality
    }
}
