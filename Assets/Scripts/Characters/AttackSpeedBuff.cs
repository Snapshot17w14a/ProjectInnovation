using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBuff : Buff
{
    public int AttackSpeed { get; }
    public override float Duration { get; }
    public AttackSpeedBuff(int attackSpeed, float duration)
    {
        AttackSpeed = Math.Max(0, attackSpeed);
        Duration = Math.Max(0, duration);
    }

    public override void OnApplied(Character character)
    {
        base.OnApplied(character);
        
    }

    public override void OnRemoved(Character character)
    {
        base.OnRemoved(character);
    }
}
