using UnityEngine;

public class MaterialSelection : CraftingProcess, ICraftingProcess
{
    private bool isMaterialSelected;
    public bool IsProcessDone => isMaterialSelected;

    private Weapon weapon;

    public void SelectMateral(int materialIndex)
    {
        weapon.SetMaterial((Weapon.Material)materialIndex);
        isMaterialSelected = true;
    }

    public void StartProcess(ref Weapon weapon)
    {
        this.weapon = weapon;
    }
}
