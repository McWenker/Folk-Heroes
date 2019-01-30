using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeaponAttackUnit : AttackUnit
{
	[SerializeField] private Weapon weapon;
	[SerializeField] private Weapon_Base weapon_Base;
	[SerializeField] private Transform handTransform;
	[SerializeField] private float windUpTime;
	private bool windUpComplete = false;
	private Dictionary<string, Vector3> handDict = new Dictionary<string, Vector3>()
    {
        {"R_southeast", new Vector3(0.572f, 0.55f, 0f)},
        {"L_southeast", new Vector3(-0.2f, 0.44f, 0f)},
        {"R_northeast", new Vector3(0.399f, 0.715f, 0f)},
        {"L_northeast", new Vector3(-0.42f, 0.579f, 0f)},

        {"R_southwest", new Vector3(-0.79f, 0.63f, 0f)},
        {"L_southwest", new Vector3(0.279f, 0.862f, 0f)},
        {"R_northwest", new Vector3(-0.28f, 0.96f, 0f)},
        {"L_northwest", new Vector3(-0.7f, 0.96f, 0f)}
    };

	public override void Attack(Action onAttackComplete)
    {
        if(attackTarget != Vector3.zero)
        {
			if(!windUpComplete)
			{
				PlaceHands();
			}
			else
			{
				windUpComplete = false;
				weapon.Attack();
				attacking = false;
				onAttackComplete();
			}
        }
    }

	public override void CommenceAttack(Vector3 target, Action Animation)
    {
		StartCoroutine(WindUp());
        attacking = true;
        attackTarget = target;
        Animation();
    }

	private void PlaceHands()
	{
        if(attackTarget.x >= transform.position.x) // facing east
        {
            if(attackTarget.z <= transform.position.z) // southeast
            {
                handTransform.localPosition = handDict["R_southeast"];
            }
            else // northeast
            {
                handTransform.localPosition = handDict["R_northeast"];
            }
        }
        else
        {
            if(attackTarget.z <= transform.position.z) // southwest
            {
                handTransform.localPosition = handDict["R_southwest"];
            }
            else // northwest
            {
                handTransform.localPosition = handDict["R_northwest"];
            }
        }
        weapon_Base.RotateWeapon(handTransform.position, attackTarget);
    }
	private IEnumerator WindUp()
	{
		yield return new WaitForSeconds(windUpTime);
		windUpComplete = true;
	}
}
