using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;

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

        var defaultCredit = PlayerPrefs.GetInt("DefaultMaterials", 0);
        if (defaultCredit == 0) CreditStartingMaterials();

        LoadWeapons();
        LoadAssemblyItems();

        var loadedpresets = Resources.LoadAll<CharacterPreset>("PetPresets");

        foreach(var preset in loadedpresets)
        {
            if (!petStats.ContainsKey(preset.name)) petStats.Add(preset.name, new CharacterStats(preset));
            if (!petPresets.ContainsKey(preset.name)) petPresets.Add(preset.name, preset);
        }

        LoadPets();
        LoadMaterials();
        LoadPotions();

        SceneManager.LoadScene("BattleScene");
    }

    public void SavePotions()
    {
        File.WriteAllText(Application.persistentDataPath + "/savedPotions.json", JsonSerializer.Serialize(potions.ConvertAll(potion => potion.GetSerializablePotion()), options));
    }

    public void LoadPotions()
    {
        var loadedPotions = JsonSerializer.Deserialize<SerializablePotion[]>(File.ReadAllText(Application.persistentDataPath + "/savedPotions.json"));
        foreach(var potion in loadedPotions)
        {
            switch (potion.Type)
            {
                case Potion.EPotion.Health:
                    AddPotion(new HealthPotion(potion.Amount));
                    continue;
                case Potion.EPotion.Damage:
                    AddPotion(new DamagePotion(potion.Amount, potion.Duration));
                    continue;
                case Potion.EPotion.Armour:
                    AddPotion(new ArmourPotion(potion.Amount, potion.Duration));
                    continue;
                case Potion.EPotion.AttackSpeed:
                    AddPotion(new AttackSpeedPotion(potion.Amount, potion.Duration));
                    continue;
                case Potion.EPotion.CriticalChance:
                    AddPotion(new CricicalChancePotion(potion.Amount, potion.Duration));
                    continue;
                case Potion.EPotion.ArmourPenetration:
                    AddPotion(new ArmourPenetrationPotion(potion.Amount, potion.Duration));
                    continue;
                default:
                    continue;
            }
        }
    }

    public void SaveWeapons()
    {
        File.WriteAllText(Application.persistentDataPath + "/savedItems.json", JsonSerializer.Serialize(weapons.ConvertAll(weapon => new SerializableWeapon(weapon)), options));
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

    private void CreditStartingMaterials()
    {
        File.Create(Application.persistentDataPath + "/savedAssemblyItems.json").Close();
        File.Create(Application.persistentDataPath + "/savedMaterials.json").Close();
        File.Create(Application.persistentDataPath + "/savedPets.json").Close();
        File.Create(Application.persistentDataPath + "/savedItems.json").Close();
        File.Create(Application.persistentDataPath + "/savedPotions.json").Close();

        assemblyItemCount.Add(AssemblyItems[0].itemName, 10);
        assemblyItemCount.Add(AssemblyItems[1].itemName, 10);

        SaveAssemblyItems();

        materialCountPair.Add(Weapon.Material.Stone, 10);
        materialCountPair.Add(Weapon.Material.Copper, 5);

        SaveMaterials();

        File.WriteAllText(Application.persistentDataPath + "/savedPets.json", "{}");
        File.WriteAllText(Application.persistentDataPath + "/savedPotions.json", "[]");
        File.WriteAllText(Application.persistentDataPath + "/savedItems.json", "[]");

        PlayerPrefs.SetInt("DefaultMaterials", 1);
        PlayerPrefs.Save();
    }

    public void EraseAllData()
    {
        File.WriteAllText(Application.persistentDataPath + "/savedMaterials.json", "");
        File.WriteAllText(Application.persistentDataPath + "/savedPets.json", "");
        File.WriteAllText(Application.persistentDataPath + "/savedAssemblyItems.json", "");
        File.WriteAllText(Application.persistentDataPath + "/savedPotions.json", "");
        File.WriteAllText(Application.persistentDataPath + "/savedItems.json", "");

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Application.Quit();
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

    public void AddPotion(Potion potion) => potions.Add(potion);

    public Potion[] GetAllPotions => potions.ToArray();
}