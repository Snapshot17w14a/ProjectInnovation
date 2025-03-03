public interface ICraftingProcess
{
    public void StartProcess(ref Item item);
    public bool IsProcessDone { get; }
}
