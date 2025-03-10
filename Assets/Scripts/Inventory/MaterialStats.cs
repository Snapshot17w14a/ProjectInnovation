using UnityEngine;

[CreateAssetMenu(fileName = "MaterialStats", menuName = "Scriptables/Material Stats")]
public class MaterialStats : ScriptableObject
{
    public int Damage;
    public int CriticalChance;
    public int CriticalDamage;
    public int AttackSpeed;
    public int ArmorPenetration;
    public Weapon.Material material;
}
    
