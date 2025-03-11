using System;

public class GlobalEvent<T> where T : Event
{
    public static Action<T> OnRaiseEvent;

    public static void RaiseEvent(T eventObject)
    {
        OnRaiseEvent?.Invoke(eventObject);
    }
}

public class Event { }

public class WeaponAssigmentEvent : Event
{
    public Weapon weapon;
    public string petName;

    public WeaponAssigmentEvent(Weapon weapon, string petName)
    {
        this.weapon = weapon;
        this.petName = petName;
    }
}
