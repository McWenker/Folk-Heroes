using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceGathererUnit : MonoBehaviour, IUnit, IGather
{
    private bool isIdle = true;
    private bool finishedMining;
    private Vector3 targetDir; 
    private Vector3 lastMoveDirection;
    private float distance;
    private Action onAnimationCompleted;
    private NavMeshAgent navAgent;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float moveSpeed;
    private List<GameResource> inventory = new List<GameResource>();
    
    private AI_Base AIBase;
    private AI_Base_Gatherer AIBaseGather;
    public void ClearMove()
    {
        navAgent.ResetPath();
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

    public void MiningCompleted()
    {
        onAnimationCompleted();
        Idling();
    }

    public void MoveTo(Vector3 target, float stopDistance, Action onArrivedAtPosition)
    {
        isIdle = false;
        navAgent.SetDestination(target);
        targetDir = (target - transform.position).normalized;
        AIBase.PlayWalkingAnimation(target - transform.position);
        
        if ((navAgent.remainingDistance != Mathf.Infinity && navAgent.remainingDistance <= stopDistance && !navAgent.pathPending)
        || Vector3.Distance(target, transform.position) <= stopDistance)
        {
            navAgent.ResetPath();
            Idling();
            onArrivedAtPosition();
        }

    }

    public void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        isIdle = false;
        AIBaseGather.PlayMiningAnimation(lookAtPosition - transform.position);
        this.onAnimationCompleted = onAnimationCompleted;
    }

    public void AddToInventory(GameResource resToAdd)
    {
        inventory.Add(resToAdd);
    }

    public void UnloadInventory()
    {
        foreach(GameResource r in inventory)
        {
            GameResourceBank.AddAmount(r.ResourceType, 1);
        }
        inventory.Clear();
    }

    private void Awake()
    {
        AIBase = GetComponent<AI_Base>();
        AIBaseGather = GetComponent<AI_Base_Gatherer>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.avoidancePriority = UnityEngine.Random.Range(0, 99);
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
