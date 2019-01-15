using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackUnit : MonoBehaviour, IUnit, IAttack
{
    private bool isIdle;

    [SerializeField] Transform moveToPoint;
    [SerializeField] float attackCooldown;
    [SerializeField] float recoveryTime;
    [SerializeField] float dashDistance;

    private Vector3 targetDir;
    private Vector3 lastMoveDirection;

    private Action onAnimationCompleted;
    private NavMeshAgent navMeshAgent;
    private AI_Base AIBase;
    private AI_Base_Enemy AIBaseEnemy;
    private bool attackReady = true;

    private void Awake()
    {
        AIBase = GetComponent<AI_Base>();
        AIBaseEnemy = GetComponent<AI_Base_Enemy>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator AttackCooldown()
    {
        attackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        attackReady = true;
    }

    private IEnumerator AttackRecovery()
    {
        yield return new WaitForSeconds(recoveryTime);
        navMeshAgent.SetDestination(moveToPoint.position);
    }

    public void AttackCompleted()
    {
        onAnimationCompleted();
        StartCoroutine(AttackCooldown());
        StartCoroutine(AttackRecovery());
    }

    public bool AttackReady()
    {
        return attackReady;
    }
    public void ClearMove()
    {
    }

    public void DashAttack(Vector3 startPos, Vector3 target, float maxDistance, Action animation)
    {
        if(Vector3.Distance(startPos, transform.position) < maxDistance)
            Vector3.Lerp(transform.position, target, Time.deltaTime);
    }

    public float DashDistance()
    {
        return dashDistance;
    }

    public void Idling()
    {
        isIdle = true;
        AIBase.PlayIdleAnimation(lastMoveDirection);
    }

    public bool IsIdle()
    {
        return isIdle;
    }

    public void MoveTo(Vector3 target, float stopDistance, Action onArrivedAtPosition)
    {
        isIdle = false;
        navMeshAgent.SetDestination(target);
        targetDir = (target - transform.position);
        AIBase.PlayWalkingAnimation(targetDir);
        if (Vector3.Distance(transform.position, target) <= stopDistance)
        {
            Idling();
            onArrivedAtPosition();
        }
    }

    public void PlayAnimationAttack(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        isIdle = false;

        AIBaseEnemy.PlayAttackAnimation(lookAtPosition - transform.position);
        this.onAnimationCompleted = onAnimationCompleted;
    }
}
