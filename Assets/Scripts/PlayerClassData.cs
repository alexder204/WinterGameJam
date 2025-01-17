using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerClass", menuName = "Scriptable Objects/PlayerClass")]
public class PlayerClassData : ScriptableObject
{
    public string className;   // The name of class
    public float attackPower;  // Attack power for the class
    public float speed;        // Speed
    public float attackDistance; // Distance of attack
}
