using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using System.Linq;
using System.IO;

public class InventoryManager : Service
{
    //Store created items, and store amout of each AssemblyItem
    private List<Item> items = new();
    private Dictionary<string, int> assemblyItemCount = new();

    //All AssemblyItems loaded from Resources/Grips
    [HideInInspector] public List<AssemblyItem> AssemblyItems { get; private set; }

    //Store the stats of each pet, accessable with its name
    private Dictionary<string, CharacterStats> petStats = new();
    private List<CharacterPreset> petPresets;

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

        AssemblyItem[] loadedAssemblyItems = Resources.LoadAll<AssemblyItem>("Grips");
        AssemblyItems = new(loadedAssemblyItems);

        items.AddRange(new Item[] { new(damage: 10, critChance: 35, armorPenetration: 1), new(damage: 1, critChance: 95, critDamage: 10) });
        SaveItems();
        LoadItems();

        assemblyItemCount.Add("Insane Grip", 2);
        assemblyItemCount.Add("Crazy Grip", 1);
        assemblyItemCount.Add("Ultimate Grip", 64);
        SaveAssemblyItems();
        LoadAssemblyItems();

        petPresets = new(Resources.LoadAll<CharacterPreset>("PetPresets"));

        foreach(var preset in petPresets)
        {
            if (!petStats.ContainsKey(preset.name)) petStats.Add(preset.name, new CharacterStats(preset));
        }

        //SavePets();
        LoadPets();
    }

    public void SaveItems()
    {
        SerializableItem[] itemDatas = new SerializableItem[items.Count];
        for(int i = 0; i < items.Count; i++) itemDatas[i] = new SerializableItem(items[i]);
        File.WriteAllText(Application.persistentDataPath + "/savedItems.json", JsonSerializer.Serialize(itemDatas, options));
    }

    public void LoadItems()
    {
        SerializableItem[] readData = JsonSerializer.Deserialize<SerializableItem[]>(File.ReadAllText(Application.persistentDataPath + "/savedItems.json"));
        Item[] loadedItems = new Item[readData.Length];
        for(int i = 0; i < readData.Length; i++) loadedItems[i] = new Item().LoadFromStruct(readData[i]);
        try { SerializableItem.StaticId = readData[^1].Id; }
        catch (System.IndexOutOfRangeException) { SerializableItem.StaticId = 0; }
        items = new(loadedItems);
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
            kvp => new CharacterStats(kvp.Value)
            );
    }

    public void AddItemToInventory(Item item) => items.Add(item);
    public void RemoveItemFromInventory(Item item) => items.Remove(item);

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

    public AssemblyItem NameToAssemblyItem(string itemName) => AssemblyItems.Where(item => item.itemName == itemName).FirstOrDefault();
}