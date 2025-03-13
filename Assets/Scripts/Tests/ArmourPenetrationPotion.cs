using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmourPenetrationPotion : Potion
{
    public ArmourPenetrationPotion(int armourPenetration, float duration)
    {
        Amount = Math.Max(0, armourPenetration);
        Duration = Math.Max(0, duration);
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.AddBuff(new ArmourPenetrationBuff(Amount, Duration));
        }
    }
}
