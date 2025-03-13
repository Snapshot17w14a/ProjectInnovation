using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CriticalChanceBuff : Buff
{
    public int CriticalChance { get; }
    public override float Duration { get; }

    public CriticalChanceBuff(int criticalChance, float duration)
    {
        CriticalChance = Math.Max(0, criticalChance);
        Duration = Math.Max(0, duration);
    }

    public override void OnApplied(Character character)
    {
        base.OnApplied(character);
        character.AddCriticalChanceStat(CriticalChance);
    }

    public override void OnRemoved(Character character)
    {
        base.OnRemoved(character);
        character.RemoveCriticalChanceStat(CriticalChance);
    }
}
