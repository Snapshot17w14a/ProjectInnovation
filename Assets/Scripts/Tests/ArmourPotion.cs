using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPotion : Potion
{
    public int ArmourAmount { get; }
    public float Duration { get; }

    public ArmourPotion(int effect, int duration)
    {
        ArmourAmount = Math.Max(0, effect);
        Duration = Math.Max(0, duration);
    }

    public override void UsePotion(Character[] characters)
    {
        foreach (Character character in characters)
        {
            
        }
    }
}
