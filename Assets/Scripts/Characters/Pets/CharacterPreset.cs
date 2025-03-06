using UnityEngine;

[System.Serializable] [CreateAssetMenu(fileName = "Character Preset", menuName = "Scriptables/Character Preset", order = 0)]
public class CharacterPreset : ScriptableObject
{
    public int Health;
    public int Damage;
    public float AttackCooldown;
    public int Defense;

    [SerializeReference]
    public Skill Skill;
    public int SkillDamage;
    public float SkillCooldown;
}
