using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPotion : Potion
{

    public ArmourPotion(int armour, float duration)
    {
        Amount = Math.Max(0, armour);
        Duration = Math.Max(0, duration);
        Type = EPotion.Armour;
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.AddBuff(new ArmourBuff(Amount, Duration));
        }
    }

    public override string Description => $"Gives {Amount} of armor for {Duration} seconds";
}
