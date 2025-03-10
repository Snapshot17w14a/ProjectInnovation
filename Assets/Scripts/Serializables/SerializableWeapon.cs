public struct SerializableWeapon
{
    public static int StaticId { get; set; } = 0;
    public int Id { get; set; }
    public Weapon.Material ItemMaterial { get; set; }
    public string Grip { get; set; }
    public int Damage { get; set; }
    public float AttackSpeed { get; set; }
    public int CritChance { get; set; }
    public int CriticalDamage { get; set; }
    public int ArmorPenetration { get; set; }

    public SerializableWeapon(Weapon item)
    {
        Id = item.Id;
        ItemMaterial = item.ItemMaterial;
        Grip = item.Grip == null ? "null" : item.Grip.itemName;
        Damage = item.Damage;
        AttackSpeed = item.AttackSpeed;
        CritChance = item.CritChance;
        CriticalDamage = item.CriticalDamage;
        ArmorPenetration = item.ArmorPenetration;
    }
}
