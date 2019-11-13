using UnityEngine;

public class InventoryEventManager
{
    public delegate void ActiveItemEvent(Object sender, Item activeItem);
    public static event ActiveItemEvent OnItemSwap;

    public static void ActiveItemSwap(Object sender, Item activeItem)
    {
        if(OnItemSwap != null) OnItemSwap(sender, activeItem);
    }
}
