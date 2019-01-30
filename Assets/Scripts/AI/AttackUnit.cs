using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackUnit : MonoBehaviour, IUnit, IAttack
{
    protected bool isIdle;

    [SerializeField] Transform moveToPoint;
    [SerializeField] float attackCooldown;
    [SerializeField] float recoveryTime;
    [SerializeField] float attackRange;

    protected Vector3 targetDir;
    protected Vector3 attackTarget;
    protected Vector3 lastMoveDirection;

    protected Action onAnimationCompleted;
    protected NavMeshAgent navMeshAgent;
    protected AI_Base AIBase;
    protected AI_Base_Soldier AIBaseSoldier;
    protected bool attackReady = true;
    protected bool attacking;    

    public bool Attacking
    {
        get { return attacking; }
        set { attacking = value; }
    }
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    public bool AttackReady
    {
        get { return attackReady; }
        set { attackReady = value; }
    }  

    public bool IsIdle
    {
        get { return isIdle; }
    }

    protected void Awake()
    {
        AIBase = GetComponent<AI_Base>();
        AIBaseSoldier = GetComponent<AI_Base_Soldier>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }
    public virtual void Attack(Action onAttackComplete)
    {
	}
    public void AttackCompleted()
    {
        attacking = false;
        attackReady = false;
        onAnimationCompleted();
        StartCoroutine(AttackCooldown());
    }
    public void ClearMove()
    {
        navMeshAgent.isStopped = true;
        moveToPoint = null;
    }

    public virtual void CommenceAttack(Vector3 target, Action Animation)
    {
        attacking = true;
        attackTarget = target;
        Animation();
    }

    public void Idling()
    {
        isIdle = true;
        AIBase.PlayIdleAnimation(lastMoveDirection);
    }

    public void MoveTo(Vector3 target, float stopDistance, Action onArrivedAtPosition)
    {
        isIdle = false;
        navMeshAgent.SetDestination(target);
        navMeshAgent.isStopped = false;
        targetDir = (target - transform.position);
        AIBase.PlayWalkingAnimation(targetDir);
        if (Vector3.Distance(transform.position, target) <= stopDistance)
        {
            Idling();
            onArrivedAtPosition();
        }
    }
    public void OnDeath()
    {
        DeathEventManager.UnitDeath(this);
    }

    public void PlayAnimationAttack(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        isIdle = false;

        AIBaseSoldier.PlayAttackAnimation(lookAtPosition - transform.position);
        this.onAnimationCompleted = onAnimationCompleted;
    }
}
