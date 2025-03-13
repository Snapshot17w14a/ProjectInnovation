using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using System.Linq;
using System.IO;

public class InventoryManager : Service
{
    //Store created items, and store amout of each AssemblyItem
    private List<Weapon> weapons = new();
    private List<Potion> potions = new();
    private Dictionary<string, int> assemblyItemCount = new();

    //All AssemblyItems loaded from Resources/Grips
    [HideInInspector] public List<AssemblyItem> AssemblyItems { get; private set; }

    //Store the stats of each pet, accessable with its name
    private Dictionary<string, CharacterStats> petStats = new();
    private readonly Dictionary<string, CharacterPreset> petPresets = new();

    //Store material amounts
    private Dictionary<Weapon.Material, int> materialCountPair = new();

    private readonly JsonSerializerOptions options = new() { WriteIndented = true };

    protected override void Awake()
    {
        if (ServiceLocator.DoesServiceExist<InventoryManager>() && !ServiceLocator.CompareService(this))
        {
            DestroyImmediate(gameObject);
            return;
        }

        ServiceLocator.RegisterService(this);
        DontDestroyOnLoad(this);

        base.Awake();

        Weapon.Initialize();

        AssemblyItem[] loadedAssemblyItems = Resources.LoadAll<AssemblyItem>("Grips");
        AssemblyItems = new(loadedAssemblyItems);

        //weapons.AddRange(new Weapon[] { new(damage: 10, critChance: 35, armorPenetration: 1), new(damage: 1, critChance: 95, critDamage: 10) });
        //SaveWeapons();
        LoadWeapons();

        //SaveAssemblyItems();
        LoadAssemblyItems();

        var loadedpresets = Resources.LoadAll<CharacterPreset>("PetPresets");

        foreach(var preset in loadedpresets)
        {
            if (!petStats.ContainsKey(preset.name)) petStats.Add(preset.name, new CharacterStats(preset));
            if (!petPresets.ContainsKey(preset.name)) petPresets.Add(preset.name, preset);
        }

        SavePets();
        LoadPets();

        materialCountPair.Add(Weapon.Material.Stone, 10);
        materialCountPair.Add(Weapon.Material.Copper, 5);

        SaveMaterials();
        LoadMaterials();
    }

    public void SavePotion(Potion potion)
    {

    }

    public void SaveWeapons()
    {
        SerializableWeapon[] itemDatas = new SerializableWeapon[weapons.Count];
        for(int i = 0; i < weapons.Count; i++) itemDatas[i] = new SerializableWeapon(weapons[i]);
        File.WriteAllText(Application.persistentDataPath + "/savedItems.json", JsonSerializer.Serialize(itemDatas, options));
    }

    public void LoadWeapons()
    {
        SerializableWeapon[] readData = JsonSerializer.Deserialize<SerializableWeapon[]>(File.ReadAllText(Application.persistentDataPath + "/savedItems.json"));
        Weapon[] loadedItems = new Weapon[readData.Length];
        for(int i = 0; i < readData.Length; i++) loadedItems[i] = new Weapon().LoadFromStruct(readData[i]);
        try { SerializableWeapon.StaticId = readData[^1].Id; }
        catch (System.IndexOutOfRangeException) { SerializableWeapon.StaticId = 0; }
        weapons = new(loadedItems);
    }

    public void SaveAssemblyItems()
    {
        File.WriteAllText(Application.persistentDataPath + "/savedAssemblyItems.json", JsonSerializer.Serialize(assemblyItemCount, options));
    }

    public void LoadAssemblyItems()
    {
        assemblyItemCount = JsonSerializer.Deserialize<Dictionary<string, int>>(File.ReadAllText(Application.persistentDataPath + "/savedAssemblyItems.json"));
    }

    public void SavePets()
    {
        var serializableDictionary = petStats.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToSerializableObject()
        );
        File.WriteAllText(Application.persistentDataPath + "/savedPets.json", JsonSerializer.Serialize(serializableDictionary, options));
    }

    public void LoadPets()
    {
        var loadedDictionary = JsonSerializer.Deserialize<Dictionary<string, SerializableCharacterStats>>(File.ReadAllText(Application.persistentDataPath + "/savedPets.json"));
        petStats = loadedDictionary.ToDictionary(
            kvp => kvp.Key,
            kvp => new CharacterStats(petPresets[kvp.Key]).FromSerializableObject(kvp.Value)
            );
    }

    public void SaveMaterials()
    {
        File.WriteAllText(Application.persistentDataPath + "/savedMaterials.json", JsonSerializer.Serialize(materialCountPair, options));
    }

    public void LoadMaterials()
    {
        materialCountPair = JsonSerializer.Deserialize<Dictionary<Weapon.Material, int>>(File.ReadAllText(Application.persistentDataPath + "/savedMaterials.json"));
    }

    public void AddItemToInventory(Weapon item) => weapons.Add(item);
    public void RemoveItemFromInventory(Weapon item) => weapons.Remove(item);

    public Weapon[] GetAllWeapons => weapons.ToArray();

    public Weapon GetWeaponFromId(int id) => weapons.Where(item => item.Id == id).First();

    public int GetAssemblyItemCount(AssemblyItem assemblyItem)
    {
        var wasValueRead = assemblyItemCount.TryGetValue(assemblyItem.itemName, out int count);
        return wasValueRead ? count : 0;
    }

    public void AddAssemblyItem(AssemblyItem assemblyItem, int amountToAdd)
    {
        bool isKeyFound = assemblyItemCount.TryGetValue(assemblyItem.itemName, out int count);
        if (!isKeyFound) assemblyItemCount.Add(assemblyItem.itemName, amountToAdd);
        else assemblyItemCount[assemblyItem.itemName] = count + amountToAdd;
    }

    public AssemblyItem NameToAssemblyItem(string itemName) => AssemblyItems.Where(item => item.itemName == itemName).First();

    public CharacterStats PetNameToStats(string petName) => petStats[petName];

    public void SetPetStat(string petName, CharacterStats statToSet)
    {
        petStats[petName] = statToSet;
    }

    public void AddMaterial(Weapon.Material material, int count) => materialCountPair[material] += count;

    public int MaterialCount(Weapon.Material material) => materialCountPair[material];
}