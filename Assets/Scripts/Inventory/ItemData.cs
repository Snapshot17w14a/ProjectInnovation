public struct ItemData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public ItemStats Stats { get; set; }

    public ItemData(string name, int id, ItemStats stats)
    {
        Name = name;
        Id = id;
        Stats = stats;
    }
}
