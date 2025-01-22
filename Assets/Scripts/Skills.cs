using UnityEngine;

[CreateAssetMenu(fileName = "Skills", menuName = "Scriptable Objects/Skills")]
public abstract class Skills : ScriptableObject
{
    public string skillName;
    public float cooldown;  // Time between uses
    public Sprite skillIcon;  // Skill icon (optional)
    private float lastUsedTime;  // Time of last skill use

    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    // Make ActivateWithCooldown virtual so it can be overridden
    public virtual void ActivateWithCooldown(PlayerSkill playerSkill)
    {
        if (IsReady())
        {
            Activate(playerSkill);
            lastUsedTime = Time.time;  // Update lastUsedTime to current time when skill is used
        }
    }

    public abstract void Activate(PlayerSkill playerSkill);

    // Initialize lastUsedTime to ensure skill is ready immediately when game starts
    public void InitializeSkill()
    {
        lastUsedTime = -cooldown;  // This ensures that the skill is ready to use immediately when the game starts
    }
}
