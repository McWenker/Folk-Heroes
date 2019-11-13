using System.Collections;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public const int actionBarSize = 10;
    [SerializeField] private int bagSize;
    [SerializeField] ActionBarController actionBars;
    [SerializeField] Transform itemSpotOnPlayer;

    private int activeActionIndex;
    private bool activeActionCooldown;
    private bool isInventoryFull;
    private Item activeItem;
    [SerializeField] Item emptyItem;
    private Item[] bagList;
    [SerializeField] private Item[] testActionBarList;
    public bool IsInventoryFull { get { return isInventoryFull; }}

    void Awake()
    {
        bagList = new Item[bagSize];
        InputEventManager.OnActionBarPress += SwapActiveAction;
        InputEventManager.OnScrollWheel += ScrollInventory;
        SetInventory();
        SwapActiveAction(this, 0);
    }

    void SetInventory()
    {
        //DEMO CODE
        itemSpotOnPlayer.gameObject.GetComponent<Item>().data = testActionBarList[0].data;
        itemSpotOnPlayer.gameObject.GetComponent<Item>().whatIsTarget = testActionBarList[0].whatIsTarget;
        activeItem = itemSpotOnPlayer.gameObject.GetComponent<Item>();
        for(int i = 0; i < testActionBarList.Length; ++i)
        {
            actionBars.SetItem(i, testActionBarList[i]);
        }
    }

    void ScrollInventory(Object sender, float direction)
    {
        Debug.Log(direction);
        int newIndex = activeActionIndex + (direction > 0f ? 1 : -1);
        if(newIndex < 0)
        {
            newIndex = testActionBarList.Length - 1;
        }
        if(newIndex >= testActionBarList.Length)
        {
            newIndex = 0;
        }
        SwapActiveAction(sender, newIndex);
    }

    void SwapActiveAction(Object sender, int index)
    {
        if(!activeActionCooldown)
        {
            if(index <= actionBarSize)
            {
                activeActionIndex = index;
                actionBars.SetActive(index);
                
                if(testActionBarList[index] != null)
                {
                    activeItem.data = testActionBarList[index].data;
                    activeItem.whatIsTarget = testActionBarList[index].whatIsTarget;
                }
                else
                {
                    activeItem.data = emptyItem.data;
                    activeItem.whatIsTarget = emptyItem.whatIsTarget;                
                }      
                
                InventoryEventManager.ActiveItemSwap(this, activeItem);
                StartCoroutine(ActionCooldown());
            }
        }
        
    }
    public bool AddItem(Item itemToAdd)
    {
        for(int i = 0; i <= actionBarSize; ++i)
        {
            if(testActionBarList[i] == null)
            {
                testActionBarList[i] = itemToAdd;
                actionBars.SetItem(i, itemToAdd);
                return true;
            }
        }
        for(int j = 0; j < bagSize; ++j)
        {
            if(bagList[j] == null)
            {
                bagList[j] = itemToAdd;
                return true;
            }
        }

        return false;
    }
    public void DropItem(bool isBag, int index)
    {
        if(isBag)
        {
            bagList[index].Remove();
            bagList[index].Drop();
            bagList[index] = null;
        }
        else
        {
            testActionBarList[index].Remove();
            testActionBarList[index].Drop();
            testActionBarList[index] = null;
        }
    }

    public void DestroyItem(bool isBag, int index)
    {
        if(isBag)
        {
            bagList[index].Remove();
            bagList[index] = null;
        }
        else
        {
            testActionBarList[index].Remove();
            testActionBarList[index] = null;
        }
    }

    private IEnumerator ActionCooldown()
    {
        activeActionCooldown = true;
        yield return new WaitForSeconds(0.1f);
        activeActionCooldown = false;
    }
}
