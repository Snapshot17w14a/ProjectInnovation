using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : CraftingProcess, ICraftingProcess
{
    public bool IsProcessDone => isProcessDone;

    private Weapon weapon;

    private bool isProcessDone = false;

    private IWeaponEffect currentEffect;

    private void Start()
    {
        //Remove after testing
        weapon = new Weapon();
        StartProcess(ref weapon);
    }

    public void ApplyNewEffect(IWeaponEffect newEffect)
    {
        if (currentEffect != null)
        {
            currentEffect.RemoveEffect();
        }

        //weapon.SetDecorationResult(newEffect);
        isProcessDone = true;
    }

    public void ClearEffect()
    {
        if (currentEffect != null)
        {
            
            currentEffect = null;
        }
    }

    public void StartProcess(ref Weapon item)
    {
        weapon = item;
    }
}
