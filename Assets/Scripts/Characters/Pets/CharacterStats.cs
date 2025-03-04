public struct CharacterStats
{
    public int Health;
    public float Armor;
    public int Damage;
    public float AttackCooldown;

    public Skill skill;
    public int SkillDamage;
    public float SkillCooldown;

    public CharacterStats(CharacterPresets preset)
    {
        Health = preset.Health;
        Damage = preset.Damage;
        Armor = preset.Armor;
        AttackCooldown = preset.AttackCooldown;
        skill = preset.Skill;
        SkillDamage = preset.SkillDamage;
        SkillCooldown = preset.SkillCooldown;
    }
}
