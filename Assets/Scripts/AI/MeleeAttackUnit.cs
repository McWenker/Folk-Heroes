using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackUnit : MonoBehaviour, IUnit, IAttack
{
    private bool isIdle;

    [SerializeField] Transform moveToPoint;
    [SerializeField] float attackCooldown;
    [SerializeField] float recoveryTime;
    [SerializeField] float dashDistance;

    private Vector3 targetDir;
    private Vector3 dashTarget;
    private Vector3 lastMoveDirection;

    private Action onAnimationCompleted;
    private NavMeshAgent navMeshAgent;
    private AI_Base AIBase;
    private AI_Base_Enemy AIBaseEnemy;
    private bool attackReady = true;
    private bool attacking;    

    public bool Attacking
    {
        get { return attacking; }
        set { attacking = value; }
    }
    public float AttackRange
    {
        get { return dashDistance; }
        set { dashDistance = value; }
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

    private void Awake()
    {
        AIBase = GetComponent<AI_Base>();
        AIBaseEnemy = GetComponent<AI_Base_Enemy>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }

    public void Attack(Action onAttackComplete)
    {
        if(dashTarget != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, dashTarget, 0.1f);
            if(Vector3.Distance(transform.position, dashTarget) <= 0.2f)
            {
                dashTarget = Vector3.zero;
                AttackCompleted();
                onAttackComplete();
            }
        }
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

    public void CommenceAttack(Vector3 target, Action Animation)
    {
        attacking = true;
        dashTarget = target;
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

        AIBaseEnemy.PlayAttackAnimation(lookAtPosition - transform.position);
        this.onAnimationCompleted = onAnimationCompleted;
    }
}
