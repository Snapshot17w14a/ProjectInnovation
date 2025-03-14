public abstract class Potion
{
    public enum EPotion
    {
        None,
        Health,
        Damage,
        Armour,
        AttackSpeed,
        CriticalChance,
        ArmourPenetration,
    }

    public int Amount { get; protected set; }
    public  float Duration { get; protected set; }
    public abstract string Description { get; }

    public EPotion Type { get; protected set; }

    public abstract void UsePotion(Character[] characters);

    public SerializablePotion GetSerializablePotion()
    {
        return new SerializablePotion(this);
    }

}
