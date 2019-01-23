using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeathEventManager
{
	public delegate void AIDeathEventHandler(IUnit sender);
	public static event AIDeathEventHandler OnUnitDeath;

	public static void UnitDeath(IUnit sender)
	{		
        if(OnUnitDeath != null) OnUnitDeath(sender);
	}
}
