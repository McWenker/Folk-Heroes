using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
	int Damage
	{
		get;
		set;
	}

	Weapon CreatorWeapon
	{
		get;
		set;
	}

	Transform AttackOrigin
	{
		get;
		set;
	}
}
