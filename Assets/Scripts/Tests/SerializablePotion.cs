using System.Text.Json.Serialization;

public class SerializablePotion
{
    [JsonPropertyName("Amount")]
    public int Amount { get; set; }
    [JsonPropertyName("Duration")]
    public float Duration { get; set; }
    [JsonPropertyName("Type")]
    public Potion.EPotion Type { get; set; }

    public SerializablePotion(Potion potion)
    {
        Amount = potion.Amount;
        Duration = potion.Duration;
        Type = potion.Type;
    }

    public SerializablePotion() { }
}
