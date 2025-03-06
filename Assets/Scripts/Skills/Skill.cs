using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public static BattleManager battleManager;

    public int SkillDamage;
    public float SkillCooldown;

    public GameObject parent;

    public virtual void FromStaticSkill(Skill skill) 
    {
        SkillDamage = skill.SkillDamage;
        SkillCooldown = skill.SkillCooldown;
    }

    public abstract void UseSkill();
}
