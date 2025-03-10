public struct SerializableCharacterStats
{
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public float AttackCooldown { get; set; }
    public int Defense { get; set; }
    public int Experience { get; set; }
    public int WeaponId { get; set; }

    public SerializableCharacterStats(CharacterStats characterStats)
    {
        MaxHealth = characterStats.MaxHealth;
        Health = characterStats.Health;
        Damage = characterStats.Damage;
        AttackCooldown = characterStats.AttackCooldown;
        Experience = characterStats.Experience;
        Defense = characterStats.Defense;
        WeaponId = characterStats.Weapon.Id;
    }
}
