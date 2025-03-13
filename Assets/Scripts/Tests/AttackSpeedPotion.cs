using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPotion : Potion
{
    public AttackSpeedPotion(int attackSpeed, float duration)
    {
        Amount = Math.Max(0, attackSpeed);
        Duration = Math.Max(0, duration);
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            character.AddBuff(new AttackSpeedBuff(Amount, Duration));
        }
    }
}
