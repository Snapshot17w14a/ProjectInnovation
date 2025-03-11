using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptables/Loot Table")]
[System.Serializable]
public class LootTable : ScriptableObject
{
    public LootTableEntry[] entries;
}

[System.Serializable]
public struct LootTableEntry
{
    public int fromLevel;

    //Material Drops
    public Weapon.Material[] materials;
    public int[] materialDropRate;
    public int[] materialRatePerLevel;

    //Ingredient Drops
    //public Ingredient[] ingredients
    //public int[] ingredientDropRate;
    //public int[] ingredientRatePerLevel;

    //Handle Drops
    public AssemblyItem[] handles;
    public int[] handleDropRate;
    public float[] handleRatePerLevel;
}