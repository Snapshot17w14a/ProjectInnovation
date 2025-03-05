public struct SerializableCharacterStats
{
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public float AttackCooldown { get; set; }
    public int Defense { get; set; }

    public string Skill { get; set; }
    public int SkillDamage { get; set; }
    public float SkillCooldown { get; set; }

    public SerializableCharacterStats(CharacterStats characterStats)
    {
        MaxHealth = characterStats.MaxHealth;
        Health = characterStats.Health;
        Damage = characterStats.Damage;
        AttackCooldown = characterStats.AttackCooldown;
        Defense = characterStats.Defense;
        Skill = characterStats.skill != null ? characterStats.skill.name : "null";
        SkillDamage = characterStats.SkillDamage;
        SkillCooldown = characterStats.SkillCooldown;
    }
}
