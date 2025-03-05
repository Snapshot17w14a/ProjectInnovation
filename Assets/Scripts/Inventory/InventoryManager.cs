using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;
using System.Linq;
using System.IO;

public class InventoryManager : Service
{
    //Store created items, and store amout of each AssemblyItem
    private readonly List<Item> items = new();
    private Dictionary<string, int> assemblyItemCount = new();

    //All AssemblyItems loaded from Resources/Grips
    [HideInInspector] public List<AssemblyItem> assemblyItems;

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
        assemblyItems = new(loadedAssemblyItems);

        //items.AddRange(new Item[] { new(damage: 10, critChance: 35, armorPenetration: 1), new(damage: 1, critChance: 95, critDamage: 10) });
        SaveItems();
        LoadItems();

        assemblyItemCount.Add("Insane Grip", 2);
        assemblyItemCount.Add("Crazy Grip", 1);
        assemblyItemCount.Add("Ultimate Grip", 64);
        SaveAssemblyItems();
        LoadAssemblyItems();
    }

    public void SaveItems()
    {
        ItemData[] itemDatas = new ItemData[items.Count];
        for(int i = 0; i < items.Count; i++) itemDatas[i] = new ItemData(items[i]);
        File.WriteAllText(Application.persistentDataPath + "/savedItems.json", JsonSerializer.Serialize(itemDatas));
    }

    public Item[] LoadItems()
    {
        ItemData[] readData = JsonSerializer.Deserialize<ItemData[]>(File.ReadAllText(Application.persistentDataPath + "/savedItems.json"));
        Item[] loadedItems = new Item[readData.Length];
        for(int i = 0; i < readData.Length; i++) loadedItems[i] = new Item().LoadFromStruct(readData[i]);
        try { ItemData.StaticId = readData[^1].Id; }
        catch (System.IndexOutOfRangeException) { ItemData.StaticId = 0; }
        return loadedItems;
    }

    public void SaveAssemblyItems()
    {
        File.WriteAllText(Application.persistentDataPath + "/savedAssemblyItems.json", JsonSerializer.Serialize(assemblyItemCount));
    }

    public void LoadAssemblyItems()
    {
        assemblyItemCount = JsonSerializer.Deserialize<Dictionary<string, int>>(File.ReadAllText(Application.persistentDataPath + "/savedAssemblyItems.json"));
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

    public AssemblyItem NameToAssemblyItem(string itemName) => assemblyItems.Where(item => item.itemName == itemName).FirstOrDefault();
}

public struct ItemData
{
    public static int StaticId { get; set; } = 0;
    public int Id { get; set; }
    public Item.Material ItemMaterial { get; set; }
    public string Grip { get; set; }
    public int Damage { get; set; }
    public int AttackSpeed { get; set; }
    public int CritChance { get; set; }
    public int CriticalDamage { get; set; }
    public int ArmorPenetration { get; set; }

    public ItemData(Item item)
    {
        Id = ++StaticId;
        ItemMaterial = item.ItemMaterial;
        Grip = item.Grip == null ? "null" : item.Grip.itemName;
        Damage = item.Damage;
        AttackSpeed = item.AttackSpeed;
        CritChance = item.CritChance;
        CriticalDamage = item.CriticalDamage;
        ArmorPenetration = item.ArmorPenetration;
    }
}
