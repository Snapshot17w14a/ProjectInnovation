using UnityEngine;

public abstract class Buff
{
    public bool IsExpired => Time.time > appliedTime + Duration;

    public abstract float Duration { get; }

    private float appliedTime;

    public virtual void OnApplied(Character character)
    {
        appliedTime = Time.time;
    }

    public virtual void OnRemoved(Character character)
    {

    }
}
