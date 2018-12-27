﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGathererUnit : MonoBehaviour, IUnit, IGather
{
    private bool isIdle;
    private bool finishedMining;
    private Vector3 targetDir;
    private Vector3 lastMoveDirection;
    private float distance;
    private Action onAnimationCompleted;
    private NavMeshAgent navAgent;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float moveSpeed;
    
    private AI_Base AIBase;
    private AI_Base_Gatherer AIBaseGather;

    public void Idling()
    {
        isIdle = true;
        AIBase.PlayIdleAnimation(lastMoveDirection);
    }

    public bool IsIdle()
    {
        return isIdle;
    }

    public void MiningCompleted()
    {
        onAnimationCompleted();
        Idling();
    }

    public void MoveTo(Vector3 target, float stopDistance, Action onArrivedAtPosition)
    {
        if(isIdle == true)
        {
            isIdle = false;
            targetDir = (target - transform.position).normalized;
            navAgent.SetDestination(target);
        }
        else if (Vector3.Distance(transform.position, target) > stopDistance)
        {
            AIBase.PlayWalkingAnimation(targetDir);
        }
        else
        {
            Idling();
            onArrivedAtPosition();
        }
    }

    public void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        isIdle = false;
        AIBaseGather.PlayMiningAnimation(lookAtPosition);
        this.onAnimationCompleted = onAnimationCompleted;
    }

    private void Awake()
    {
        AIBase = GetComponent<AI_Base>();
        AIBaseGather = GetComponent<AI_Base_Gatherer>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private bool CanMove(Vector3 dir, float distance)
    {
        Debug.DrawRay(rayPoint.position, dir, Color.red, 2f);
        return !Physics.Raycast(rayPoint.position, dir, distance);
    }

    private bool TryMove(Vector3 baseMoveDir, float distance)
    {
        Vector3 moveDir = baseMoveDir;
        bool canMove = CanMove(moveDir, moveSpeed * Time.deltaTime);
        if (!canMove)
        {
            //cannot move diagonally
            moveDir = new Vector3(baseMoveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0f && CanMove(moveDir, moveSpeed * Time.deltaTime);
            if (!canMove)
            {
                //cannot move horizontally
                moveDir = new Vector3(0f, 0f, baseMoveDir.y).normalized;
                canMove = CanMove(moveDir, moveSpeed * Time.deltaTime);
                if(!canMove)
                {
                    //cannot move, must redirect moveDir
                    moveDir = Vector3.forward;
                    canMove = CanMove(moveDir, moveSpeed * Time.deltaTime);
                }
            }
        }

        if (canMove)
        {
            lastMoveDirection = moveDir;
            transform.position = Vector3.MoveTowards(transform.position, transform.position+moveDir, moveSpeed * Time.deltaTime);
            return true;
        }

        else
        {
            return false;
        }
    }
}
