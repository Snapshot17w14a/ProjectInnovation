using System;

public class DamagePotion : Potion
{

    public DamagePotion(int damage, float duration)
    {
        Amount = Math.Max(0, damage);
        Duration = Math.Max(0, duration);
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.AddBuff(new DamageBuff(Amount, Duration));
        }
    }
}
