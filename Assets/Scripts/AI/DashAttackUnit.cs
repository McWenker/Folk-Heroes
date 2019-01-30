using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashAttackUnit : AttackUnit
{
    public override void Attack(Action onAttackComplete)
    {
        if(attackTarget != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, attackTarget, 0.1f);
            if(Vector3.Distance(transform.position, attackTarget) <= 0.2f)
            {
                attackTarget = Vector3.zero;
                AttackCompleted();
                onAttackComplete();
            }
        }
    }
}
