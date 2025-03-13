using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmourPenetrationBuff : Buff
{
    public int Damage { get; }
    public override float Duration { get; }

    public ArmourPenetrationBuff(int armourPenetration, float duration)
    {
        Damage = Math.Max(0, armourPenetration);
        Duration = Math.Max(0, duration);
    }

    public override void OnApplied(Character character)
    {
        base.OnApplied(character);
        character.AddArmourPenetrationStat(Damage);
    }

    public override void OnRemoved(Character character)
    {
        base.OnRemoved(character);
        character.RemoveArmourPenetrationStat(Damage);
    }
}
