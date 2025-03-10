using System;

public class CharacterStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;
    public float AttackCooldown;
    public int Defense;
    public int Experience;

    public Skill skill;
    public Weapon Weapon;

    public string Name;

    private readonly CharacterPreset usedPreset;

    public int Level
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
        Weapon = null;
        usedPreset = preset;
        Name = preset.name;
        GlobalEvent<WeaponAssigmentEvent>.OnRaiseEvent += OnAssignmentEvent;
        ApplyLevelBuffs();
    }

    ~CharacterStats()
    {
        GlobalEvent<WeaponAssigmentEvent>.OnRaiseEvent -= OnAssignmentEvent;
    }

    public CharacterStats FromSerializableObject(SerializableCharacterStats serializedObject)
    {
        MaxHealth = serializedObject.MaxHealth;
        Health = serializedObject.Health;
        Damage = serializedObject.Damage;
        AttackCooldown = serializedObject.AttackCooldown;
        Experience = serializedObject.Experience;
        Defense = serializedObject.Defense;
        Weapon = serializedObject.WeaponId != -1 ? ServiceLocator.GetService<InventoryManager>().GetWeaponFromId(serializedObject.WeaponId) : null;
        return this;
    }

    public SerializableCharacterStats ToSerializableObject()
    {
        return new SerializableCharacterStats(this);
    }

    public void ApplyLevelBuffs()
    {
        int level = Level - 1;
        Damage += usedPreset.DmgPerLevel * level;
        Health += usedPreset.HpPerLevel * level;
        MaxHealth += usedPreset.HpPerLevel * level;
    }

    public CharacterStats AssignWeapon(Weapon weapon)
    {
        AddWeaponStats(true);
        Weapon = weapon;
        AddWeaponStats();
        return this;
    }

    public CharacterStats AssignWeapon(int id) 
    {
        AddWeaponStats(true);
        Weapon = ServiceLocator.GetService<InventoryManager>().GetWeaponFromId(id);
        AddWeaponStats();
        return this;
    }

    private void AddWeaponStats(bool isRemoving = false)
    {
        if (Weapon == null) return;
        int modifier = isRemoving ? -1 : 1;
        Damage += Weapon.Damage * modifier;
        AttackCooldown += Weapon.AttackSpeed * modifier;
    }

    private void OnAssignmentEvent(WeaponAssigmentEvent eventParams)
    {
        if (Name != eventParams.petName) return;
        AssignWeapon(eventParams.weapon);

        ServiceLocator.GetService<InventoryManager>().SetPetStat(Name, this);
    }
}
