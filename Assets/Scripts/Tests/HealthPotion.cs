﻿using System;

public class HealthPotion : Potion
{
    public HealthPotion(int healAmount)
    {
        Amount = Math.Max(0, healAmount);
        Type = EPotion.Health;
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.Heal(Amount);
        }
    }

    public override string Description => $"Heals {Amount}.";
}
