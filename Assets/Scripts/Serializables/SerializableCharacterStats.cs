public struct SerializableCharacterStats
{
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public float AttackCooldown { get; set; }
    public int Defense { get; set; }
    public int Experience { get; set; }
    public int WeaponId { get; set; }
    public string Name { get; set; }

    public SerializableCharacterStats(CharacterStats characterStats)
    {
        MaxHealth = characterStats.MaxHealth;
        Health = characterStats.Health;
        Damage = characterStats.Damage;
        AttackCooldown = characterStats.attackCooldown;
        Experience = characterStats.Experience;
        Defense = characterStats.Defense;
        WeaponId = characterStats.Weapon != null ? characterStats.Weapon.Id : -1;
        Name = characterStats.Name;
    }
}
