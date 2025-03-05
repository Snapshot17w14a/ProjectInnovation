public struct CharacterStats
{
    public int MaxHealth;
    public int Health;
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
        AttackCooldown = preset.AttackCooldown;
        Defense = preset.Defense;
        skill = preset.Skill;
        SkillDamage = preset.SkillDamage;
        SkillCooldown = preset.SkillCooldown;
    }

    public CharacterStats(SerializableCharacterStats serializedObject)
    {
        MaxHealth = serializedObject.MaxHealth;
        Health = serializedObject.Health;
        Damage = serializedObject.Damage;
        AttackCooldown = serializedObject.AttackCooldown;
        Defense = serializedObject.Defense;
        skill = null;
        SkillDamage = serializedObject.SkillDamage;
        SkillCooldown = serializedObject.SkillCooldown;
    }

    public SerializableCharacterStats ToSerializableObject()
    {
        return new SerializableCharacterStats(this);
    }
}
