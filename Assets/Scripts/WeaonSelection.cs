public class WeaponSelection : CraftingProcess, ICraftingProcess
{
    private bool isWeaponSelected;
    public bool IsProcessDone => isWeaponSelected;

    private Weapon weapon;

    public void SelectWeapon()
    {
        isWeaponSelected = true;
    }

    public void StartProcess(ref Weapon weapon)
    {
        this.weapon = weapon;
    }
}
