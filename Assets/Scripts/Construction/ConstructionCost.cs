using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionCost : MonoBehaviour
{
	public int goldCost;
	public int ironCost;
	public int manaCost;
	public int bloodCost;

	public bool CanAfford()
	{
		if(GameResourceBank.GetAmount(GameResourceType.Gold) < goldCost)
			return false;
		if(GameResourceBank.GetAmount(GameResourceType.Iron) < ironCost)
			return false;
		if(GameResourceBank.GetAmount(GameResourceType.Mana) < manaCost)
			return false;
		if(GameResourceBank.GetAmount(GameResourceType.Blood) < bloodCost)
			return false;
		return true;
	}

	public void Spend()
	{
		GameResourceBank.AddAmount(GameResourceType.Gold, -goldCost);
		GameResourceBank.AddAmount(GameResourceType.Iron, -ironCost);
		GameResourceBank.AddAmount(GameResourceType.Mana, -manaCost);
		GameResourceBank.AddAmount(GameResourceType.Blood, -bloodCost);
	}
}
