public interface ICraftingProcess
{
    public void StartProcess(ref Weapon item);
    public bool IsProcessDone { get; }
}
