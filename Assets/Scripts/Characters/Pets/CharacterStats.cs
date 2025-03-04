public struct CharacterStats
{
    public int MaxHealth;
    public int Health;
    //public float Armor;
    public int Damage;
    public float AttackCooldown;
    public int Defense;

    public Skill skill;
    public int SkillDamage;
    public float SkillCooldown;

    public CharacterStats(CharacterPreset preset)
    {
        MaxHealth = preset.Health;
        Health = preset.Health;
        Damage = preset.Damage;
        //Armor = preset.Armor;
        AttackCooldown = preset.AttackCooldown;
        Defense = preset.Defense;
        skill = preset.Skill;
        SkillDamage = preset.SkillDamage;
        SkillCooldown = preset.SkillCooldown;
    }
}
