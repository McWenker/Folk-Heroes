using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResourceBank
{
    public static event EventHandler OnGoldAmountChanged;
    public static event EventHandler OnIronAmountChanged;
    public static event EventHandler OnManaAmountChanged;
    public static event EventHandler OnBloodAmountChanged;
    private static int goldAmount = 20;
    private static int ironAmount = 20;
    private static int manaAmount = 20;
    private static int bloodAmount = 20;

    public static void AddAmount(GameResourceType resourceToGrow, int amount)
    {
        switch (resourceToGrow)
        {
            case (GameResourceType.Gold):
                goldAmount += amount;
                if (OnGoldAmountChanged != null) OnGoldAmountChanged(null, EventArgs.Empty);
                break;
            case (GameResourceType.Iron):
                ironAmount += amount;
                if (OnIronAmountChanged != null) OnIronAmountChanged(null, EventArgs.Empty);
                break;
            case (GameResourceType.Mana):
                manaAmount += amount;
                if (OnManaAmountChanged != null) OnManaAmountChanged(null, EventArgs.Empty);
                break;
            case (GameResourceType.Blood):
                bloodAmount += amount;
                if (OnBloodAmountChanged != null) OnBloodAmountChanged(null, EventArgs.Empty);
                break;
        }
    }

    public static int GetAmount(GameResourceType resourceToGet)
    {
        switch (resourceToGet)
        {
            case (GameResourceType.Gold):
                return goldAmount;
            case (GameResourceType.Iron):
                return ironAmount;
            case (GameResourceType.Mana):
                return manaAmount;
            case (GameResourceType.Blood):
                return bloodAmount;
            default:
                return 0;
        }
    }
}
