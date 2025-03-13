using System;

public class HealthPotion : Potion
{
    public int HealAmount { get; }
    public HealthPotion(int healAmount)
    {
        HealAmount = Math.Max(0, healAmount);
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.Heal(HealAmount);
        }
    }
}
