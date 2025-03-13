using System;

public class DamageBuff : Buff
{
    public int Damage { get; }
    public override float Duration { get; }

    public DamageBuff(int damage, float duration)
    {
        Damage = Math.Max(0, damage);
        Duration = Math.Max(0,duration);
    }

    public override void OnApplied(Character character)
    {
        base.OnApplied(character);
        character.AddDamageStat(Damage);
    }

    public override void OnRemoved(Character character)
    {
        base.OnRemoved(character);
        character.RemoveDamageStat(Damage);
    }
}
