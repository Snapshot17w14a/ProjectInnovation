using System.Linq;
using UnityEngine;

public class Weapon
{
    private static MaterialStats[] materialStats;
    public int Id { get;private set; }
    public Material ItemMaterial { get; private set; }
    public AssemblyItem Grip { get; private set; }
    public int Damage { get; private set; } = 0;
    public float AttackSpeed { get; private set; } = 0;
    public int CritChance { get; private set; } = 0;
    public int CriticalDamage { get; private set; } = 0;
    public int ArmorPenetration { get; private set; } = 0;

    public Weapon(Material material = Material.None, AssemblyItem grip = null, int damage = 0, int attackSpeed = 0, int critChance = 0, int critDamage = 0, int armorPenetration = 0)
    {
        Id = ++SerializableWeapon.StaticId;
        ItemMaterial = material;
        Grip = grip;
        Damage = damage;
        AttackSpeed = attackSpeed;
        CritChance = critChance;
        CriticalDamage = critDamage;
        ArmorPenetration = armorPenetration;
    }

    public Weapon LoadFromStruct(SerializableWeapon data)
    {
        Id = data.Id;
        ItemMaterial = data.ItemMaterial;
        Grip = ServiceLocator.GetService<InventoryManager>().NameToAssemblyItem(data.Grip);
        Damage = data.Damage;
        AttackSpeed = data.AttackSpeed;
        CritChance = data.CritChance;
        CriticalDamage = data.CriticalDamage;
        ArmorPenetration = data.ArmorPenetration;
        return this;
    }

    public void SetForgeResult(int score)
    {
        Debug.Log($"Setting forge result: {score}, initial attack speed {AttackSpeed}");
        AttackSpeed *= (0.9f + (score - 1) * 0.05f);
        Debug.Log($"end attack speed {AttackSpeed}");
    }

    public void SetCastResult(int score)
    {
        Debug.Log($"Setting cast result: {score}, initial damage {Damage}");
        Damage = Mathf.RoundToInt(Damage * (0.9f + (score - 1) * 0.05f));
        Debug.Log($"end damage {Damage}");
    }

    public void SetHammerResult(int score)
    {
        CritChance += -10 + (score - 1) * 5;
    }

    public void SetGrip(AssemblyItem grip)
    {
        Grip = grip;
    }

    public void SetMaterial(Material material)
    {
        ItemMaterial = material;
        MaterialStats stats = materialStats.Where(arrMaterial => arrMaterial.material == material).FirstOrDefault();
        Damage = stats.Damage;
        AttackSpeed = stats.AttackSpeed;
        CritChance = stats.CriticalChance;
        CriticalDamage = stats.CriticalDamage;
        ArmorPenetration = stats.ArmorPenetration;
    }

    public static void Initialize()
    {
        materialStats = Resources.LoadAll<MaterialStats>("MaterialStats");
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
