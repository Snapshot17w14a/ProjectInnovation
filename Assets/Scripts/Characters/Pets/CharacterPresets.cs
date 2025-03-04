using UnityEngine;

[CreateAssetMenu(fileName = "Character Preset", menuName = "Scriptables/Character Preset", order = 0)]
public class CharacterPresets : ScriptableObject
{
    public int Health;
    public float Armor;
    public int Damage;
    public float AttackCooldown;

    public Skill Skill;
    public int SkillDamage;
    public float SkillCooldown;
}
