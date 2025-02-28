using UnityEngine;

public class Item
{
    public Material ItemMaterial { get; private set; }
    public AssemblyItem Grip { get; private set; }
    public int Damage { get; private set; } = 0;
    public int AttackSpeed { get; private set; } = 0;
    public int CritChance { get; private set; } = 0;
    public int CriticalDamage { get; private set; } = 0;
    public int ArmorPenetration { get; private set; } = 0;

    public Item(Material material = Material.None, AssemblyItem grip = null, int damage = 0, int attackSpeed = 0, int critChance = 0, int critDamage = 0, int armorPenetration = 0)
    {
        ItemMaterial = material;
        Grip = grip;
        Damage = damage;
        AttackSpeed = attackSpeed;
        CritChance = critChance;
        CriticalDamage = critDamage;
        ArmorPenetration = armorPenetration;
    }

    public Item LoadFromStruct(ItemData data)
    {
        ItemMaterial = data.ItemMaterial;
        Grip = data.Grip;
        Damage = data.Damage;
        AttackSpeed = data.AttackSpeed;
        CritChance = data.CritChance;
        CriticalDamage = data.CriticalDamage;
        ArmorPenetration = data.ArmorPenetration;
        return this;
    }

    public void SetForgeResult(int score)
    {
        AttackSpeed = Mathf.RoundToInt(AttackSpeed * (0.9f + (score - 1) * 0.05f));
    }

    public void SetCastResult(int score)
    {
        Damage = Mathf.RoundToInt(Damage * (0.9f + (score - 1) * 0.05f));
    }

    public void SetHammerResult(int score)
    {
        CritChance += -10 + (score - 1) * 5;
    }

    public enum Material
    {
        None = -1,
        Stone,
        Copper,
        Iron,
        Gold,
        Platinum,
        Diamond
    }
}
