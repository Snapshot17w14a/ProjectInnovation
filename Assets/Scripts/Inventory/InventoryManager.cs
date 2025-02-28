using UnityEngine;
using System.Text.Json;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    Dictionary<int, ItemData> idItemPair = new();

    // Start is called before the first frame update
    void Start()
    {
        idItemPair.Add(2, new ItemData("asdsad", 2));
        idItemPair.Add(1, new ItemData("feaf", 1));
        idItemPair.Add(4, new ItemData("ttsdf", 4));
        idItemPair.Add(3, new ItemData("rthgd", 3));

        Debug.Log(JsonSerializer.Serialize(idItemPair));
    }

}

public struct ItemData
{
    public string name;
    public int id;

    public ItemData(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}
