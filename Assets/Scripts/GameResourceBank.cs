using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResourceBank
{
    public static event EventHandler OnGoldAmountChanged;
    public static event EventHandler OnIronAmountChanged;
    public static event EventHandler OnManaAmountChanged;
    public static event EventHandler OnStoneAmountChanged;
    private static int goldAmount;
    private static int ironAmount;
    private static int manaAmount;
    private static int stoneAmount;

    public static void AddAmount(GameResource resourceToGrow, int amount)
    {
        switch (resourceToGrow)
        {
            case (GameResource.Gold):
                goldAmount += amount;
                if (OnGoldAmountChanged != null) OnGoldAmountChanged(null, EventArgs.Empty);
                break;
            case (GameResource.Iron):
                ironAmount += amount;
                if (OnIronAmountChanged != null) OnIronAmountChanged(null, EventArgs.Empty);
                break;
            case (GameResource.Mana):
                manaAmount += amount;
                if (OnManaAmountChanged != null) OnManaAmountChanged(null, EventArgs.Empty);
                break;
            case (GameResource.Stone):
                stoneAmount += amount;
                if (OnStoneAmountChanged != null) OnStoneAmountChanged(null, EventArgs.Empty);
                break;
        }
    }

    public static int GetAmount(GameResource resourceToGet)
    {
        switch (resourceToGet)
        {
            case (GameResource.Gold):
                return goldAmount;
            case (GameResource.Iron):
                return ironAmount;
            case (GameResource.Mana):
                return manaAmount;
            case (GameResource.Stone):
                return stoneAmount;
            default:
                return 0;
        }
    }
}
