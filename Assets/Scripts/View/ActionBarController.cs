using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarController : MonoBehaviour
{
    [SerializeField] ActionBarItem[] barItems = new ActionBarItem[PlayerInventory.actionBarSize];
    [SerializeField] Color defaultItemColor;
    [SerializeField] Color activeItemColor;

    public void ClickToSet(int index)
    {
        InputEventManager.ActionBarPress(this, index);
    }
    public void SetActive(int index)
    {
        for(int i = 0; i < barItems.Length; ++i)
        {
            barItems[i].Color(i == index ? activeItemColor : defaultItemColor);
        }
    }
    public void SetItem(int index, Item itemToSet)
    {
        if(itemToSet != null)
            barItems[index].Set(itemToSet.data.itemSprites.inventorySprite);
        else
            barItems[index].Set(null);
    }
}
