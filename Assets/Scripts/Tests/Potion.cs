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

    public int Amount { get; set; }
    public  float Duration { get; set; }

    public abstract void UsePotion(Character[] characters);
}
