public struct CharacterStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;
    public float AttackCooldown;
    public int Defense;
    public int Experience;

    public Skill skill;

    private CharacterPreset usedPreset;

    public readonly int Level
    {
        get
        {
            int threshold = 100;
            int level = 1;
            int xp = Experience;
            while (Experience > threshold)
            {
                level++;
                xp -= threshold;
                threshold += 100;
            }
            return level;
        }
    }

    public CharacterStats(CharacterPreset preset)
    {
        MaxHealth = preset.Health;
        Health = preset.Health;
        Damage = preset.Damage;
        AttackCooldown = preset.AttackCooldown;
        Defense = preset.Defense;
        Experience = 0;
        for (int i = 0; i < preset.StartingLevel; i++) Experience += 100 + (i - 1) * 100;
        skill = preset.Skill;
        usedPreset = preset;
    }

    public CharacterStats FromSerializableObject (SerializableCharacterStats serializedObject)
    {
        MaxHealth = serializedObject.MaxHealth;
        Health = serializedObject.Health;
        Damage = serializedObject.Damage;
        AttackCooldown = serializedObject.AttackCooldown;
        Experience = serializedObject.Experience;
        Defense = serializedObject.Defense;
        return this;
    }

    public readonly SerializableCharacterStats ToSerializableObject()
    {
        return new SerializableCharacterStats(this);
    }

    public void ApplyLevelBuffs()
    {
        int level = Level - 1;
        Damage += usedPreset.DmgPerLevel * level;
        Health += usedPreset.HpPerLevel * level;
    }
}
