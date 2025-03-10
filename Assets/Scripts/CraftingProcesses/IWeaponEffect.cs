using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponEffect
{
    public void ApplyEffect(Enemy enemy);
    public void RemoveEffect(Enemy enemy);
}
