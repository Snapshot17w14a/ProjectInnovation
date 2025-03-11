using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : CraftingProcess, ICraftingProcess
{
    public bool IsProcessDone => isProcessDone;

    private Weapon weapon;

    private bool isProcessDone = false;

    public void CompleteProcess(IWeaponEffect newEffect)
    {
        weapon.SetDecorationResult(newEffect);
        isProcessDone = true;
    }

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }
}
