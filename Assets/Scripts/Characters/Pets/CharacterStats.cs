public struct CharacterStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;
    public float AttackCooldown;
    public int Defense;

    public Skill skill;

    public CharacterStats(CharacterPreset preset)
    {
        MaxHealth = preset.Health;
        Health = preset.Health;
        Damage = preset.Damage;
        AttackCooldown = preset.AttackCooldown;
        Defense = preset.Defense;
        skill = preset.Skill;
    }

    public CharacterStats FromSerializableObject (SerializableCharacterStats serializedObject)
    {
        MaxHealth = serializedObject.MaxHealth;
        Health = serializedObject.Health;
        Damage = serializedObject.Damage;
        AttackCooldown = serializedObject.AttackCooldown;
        Defense = serializedObject.Defense;
        return this;
    }

    public readonly SerializableCharacterStats ToSerializableObject()
    {
        return new SerializableCharacterStats(this);
    }
}
