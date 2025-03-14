using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CricicalChancePotion : Potion
{
    public CricicalChancePotion(int criticalChance, float duration)
    {
        Amount = Math.Max(0, criticalChance);
        Duration = Math.Max(0, duration);
        Type = EPotion.CriticalChance;
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.AddBuff(new CriticalChanceBuff(Amount, Duration));
        }
    }

    public override string Description => $"Gives {Amount} critical chance for {Duration} seconds.";
}
