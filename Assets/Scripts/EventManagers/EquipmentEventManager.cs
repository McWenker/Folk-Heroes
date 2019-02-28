using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentEventManager
{
	public delegate void EquipmentEventHandler(Object sender, bool isEquipped);
    public static event EquipmentEventHandler OnEquip;

    public static void ToggleWeapons(Object sender, bool isEquipped)
    {
        if (OnEquip != null) OnEquip(sender, isEquipped);
    }
}
