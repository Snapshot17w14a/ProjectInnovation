using System;

public class DamagePotion : Potion
{
    public int DamageAmount { get; }
    public float Duration { get; }

    public DamagePotion(int damage, float duration)
    {
        DamageAmount = Math.Max(0, damage);
        Duration = Math.Max(0, duration);
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.AddBuff(new DamageBuff(DamageAmount, Duration));
        }
    }
}
