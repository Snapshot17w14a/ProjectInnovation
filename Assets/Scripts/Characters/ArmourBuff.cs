using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBuff : Buff
{
    public int Armour { get; }
    public override float Duration { get; }

    public ArmourBuff(int armour, float duration)
    {
        Armour = Math.Max(0, armour);
        Duration = Math.Max(0, duration);
    }

    public override void OnApplied(Character character)
    {
        base.OnApplied(character);
        character.AddArmourStat(Armour);
    }

    public override void OnRemoved(Character character)
    {
        base.OnRemoved(character);
        character.RemoveArmourStat(Armour);
    }
}
