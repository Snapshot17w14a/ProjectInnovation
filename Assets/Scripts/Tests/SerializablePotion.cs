public class SerializablePotion
{
    public int Amount { get; set; }
    public float Duration { get; set; }
    public Potion.Type Type { get; set; }

    //public SerializablePotion(Potion potion)
    //{
    //    Amount = potion.Amount;
    //    Duration = potion.Duration;
    //    Type = potion.Type;
    //}
}
