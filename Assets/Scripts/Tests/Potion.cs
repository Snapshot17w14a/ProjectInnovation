public abstract class Potion
{
    public enum Type
    {
        Health,
        Armor,
        Damage
    }

    public abstract void UsePotion(Character[] characters);
}
