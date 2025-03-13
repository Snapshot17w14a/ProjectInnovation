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

    public EPotion Type { get; protected set; }

    public abstract void UsePotion(Character[] characters);
}
