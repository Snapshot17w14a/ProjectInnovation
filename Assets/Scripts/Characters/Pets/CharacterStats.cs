using System;

public class CharacterStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;
    public int Defense;
    public int Experience = 0;

    private int levelThreshold = 100;

    public Skill skill;
    public Weapon Weapon;

    public string Name;

    private readonly CharacterPreset usedPreset;

    public float AttackCooldown;

    private const float iceEffectExtraCooldownFraction = 0.1f;

    public Action OnWeaponChanged;
    public int Level { get; private set; } = 1;

    //public int Level
    //{
    //    get
    //    {
    //        int threshold = 100;
    //        int level = 1;
    //        int xp = Experience;
    //        while (Experience > threshold)
    //        {
    //            level++;
    //            xp -= threshold;
    //            threshold += 100;
    //        }
    //        return level;
    //    }
    //}

    public CharacterStats(CharacterPreset preset)
    {
        MaxHealth = preset.Health;
        Health = preset.Health;
        Damage = preset.Damage;
        AttackCooldown = preset.AttackCooldown;
        Defense = preset.Defense;
        usedPreset = preset;
        int xp = 0;
        for (int i = 0; i < preset.StartingLevel - 1; i++) xp += 100 + i * 100;
        AddExperience(xp);
        skill = preset.Skill;
        Weapon = null;
        Name = preset.name;
        GlobalEvent<WeaponAssigmentEvent>.OnRaiseEvent += OnAssignmentEvent;
    }

    private CharacterStats(CharacterStats original)
    {
        MaxHealth = original.MaxHealth;
        Health = original.MaxHealth;
        Damage = original.Damage;
        AttackCooldown = original.AttackCooldown;
        Defense = original.Defense;
        usedPreset = original.usedPreset;
        Level = original.Level;
        Weapon = original.Weapon;
        Name = original.Name;
        skill = original.skill;
        Experience = original.Experience;
        levelThreshold = original.levelThreshold;
        GlobalEvent<WeaponAssigmentEvent>.OnRaiseEvent += OnAssignmentEvent;
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
        Damage += usedPreset.DmgPerLevel;
        Health += usedPreset.HpPerLevel;
        MaxHealth += usedPreset.HpPerLevel;
    }

    public CharacterStats AssignWeapon(Weapon weapon)
    {
        AddWeaponStats(true);
        Weapon = weapon;
        AddWeaponStats();
        OnWeaponChanged?.Invoke();
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

        var invManager = ServiceLocator.GetService<InventoryManager>();
        invManager.SetPetStat(Name, this);
        invManager.SavePets();
    }

    public void AddExperience(int xp)
    {
        Experience += xp;
        while (Experience >= levelThreshold)
        {
            Experience -= levelThreshold;
            Level++;
            levelThreshold += 100;
            ApplyLevelBuffs();
        }
    }

    public CharacterStats Clone() => new(this);
}
