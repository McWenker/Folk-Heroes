using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackUnit : MonoBehaviour, IUnit, IAttack
{
    private bool isIdle;

    [SerializeField] Transform moveToPoint;
    [SerializeField] float aggroRange;
    [SerializeField] float attackCooldown;
    [SerializeField] float recoveryTime;
    [SerializeField] float dashDistance;

    private Vector3 targetDir;
    private Vector3 lastMoveDirection;

    private Action onAnimationCompleted;
    private NavMeshAgent navMeshAgent;
    private AI_Base AIBase;
    private AI_Base_Enemy AIBaseEnemy;

    private bool hasFoeCheckedRecently;
    private bool hasDistanceCheckedRecently;
    private bool attackReady = true;
    int layerMask;

    private List<RaycastHit> hitList = new List<RaycastHit>();

    private void Awake()
    {
        AIBase = GetComponent<AI_Base>();
        AIBaseEnemy = GetComponent<AI_Base_Enemy>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        layerMask = ~((1 << 11) | (1 << 13));
        EnemyEventManager.OnAggro += CheckGroupAggroDistance;
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    private Transform AssignAggroTarget(Transform target)
    {
        moveToPoint = target;
        if (moveToPoint != null)
            EnemyEventManager.NewAggro(gameObject, moveToPoint);
        return moveToPoint;
    }

    private Transform CheckForFoes()
    {
        for (int i = 0; i < 72; ++i)
        {
            float angle = i * 5;
            Ray ray = new Ray();
            ray.origin = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            ray.direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
            RaycastHit raycastHit = new RaycastHit();
            Debug.DrawRay(ray.origin, ray.direction * aggroRange, Color.black, .75f);
            if (Physics.Raycast(ray, out raycastHit, aggroRange, layerMask))
            {
                /*if (raycastHit.collider.gameObject.layer == 9)
                {*/
                hitList.Add(raycastHit);
                //}
            }
        }

        return AssignAggroTarget(DetermineClosestTarget());
    }

    private void CheckGroupAggroDistance(GameObject sender, Transform target)
    {
        // negligible cases
        if (this == null)
            return;
        if (sender == null)
            return;
        if (sender == gameObject)
            return;
        
        if (moveToPoint == null)
        {
            if (Vector3.Distance(transform.position, sender.transform.position) < (aggroRange / 2))
                StartCoroutine(DelayedAggroAssignment(target));
        }
    }

    private Transform DetermineClosestTarget()
    {
        Transform closestFoeTransform = null;
        if (hitList.Count > 0)
        {
            foreach (RaycastHit rcH in hitList)
            {
                if (closestFoeTransform == null)
                    closestFoeTransform = rcH.collider.transform;
                else if (Vector3.Distance(transform.position, closestFoeTransform.position) > Vector3.Distance(transform.position, rcH.collider.transform.position))
                    closestFoeTransform = rcH.collider.transform;
            }
        }

        hitList.Clear();
        return closestFoeTransform;
    }

    private bool CheckForDistanceBreak()
    {
        if (Vector3.Distance(transform.position, moveToPoint.position) > aggroRange * 2f)
        {
            AssignAggroTarget(null);
            return false;
        }
        return true;
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

    private IEnumerator CheckForDistanceBreakCooldown()
    {
        hasDistanceCheckedRecently = true;
        yield return new WaitForSeconds(1.5f);
        hasDistanceCheckedRecently = false;
    }

    private IEnumerator CheckForFoesCooldown()
    {
        hasFoeCheckedRecently = true;
        yield return new WaitForSeconds(0.7f);
        hasFoeCheckedRecently = false;
    }

    private IEnumerator DelayedAggroAssignment(Transform target)
    {
        yield return new WaitForSeconds(0.35f);
        AssignAggroTarget(target);
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

    public void DashAttack(Vector3 startPos, Vector3 target, float maxDistance, Action animation)
    {
        if(Vector3.Distance(startPos, transform.position) < maxDistance)
            Vector3.Lerp(transform.position, target, Time.deltaTime);
    }

    public float DashDistance()
    {
        return dashDistance;
    }
    
    public bool DistanceCheck()
    {
        if (!hasDistanceCheckedRecently)
        {
            StartCoroutine(CheckForDistanceBreakCooldown());
            return CheckForDistanceBreak();
        }
        return true;
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
        targetDir = (target - transform.position).normalized;
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

        AIBaseEnemy.PlayAttackAnimation(lookAtPosition);
        this.onAnimationCompleted = onAnimationCompleted;
    }

    public Transform SearchForFoes()
    {
        if (!hasFoeCheckedRecently)
        {
            StartCoroutine(CheckForFoesCooldown());
            return CheckForFoes();
        }
        return null;
    }
}
