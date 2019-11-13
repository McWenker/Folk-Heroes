public interface I_Stackable
{
    int MaxStackSize { get; set; }
    int StackCount { get; set; }
    void IncreaseStack();
    void DecreaseStack();
}
