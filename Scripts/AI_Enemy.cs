using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Enemy : MonoBehaviour
{
    [SerializeField] Transform moveToPoint;
    [SerializeField] float aggroRange;
    Vector3 homePoint;
    private NavMeshAgent navMeshAgent;
    private bool hasFoeCheckedRecently;
    private bool hasDistanceCheckedRecently;
    [SerializeField] LayerMask[] layerMasks;
    int layerMask;

    private List<RaycastHit> hitList = new List<RaycastHit>();

    private void Awake()
    {
        layerMask = ~((1 << 11) | (1 << 13));
        EnemyEventManager.OnAggro += CheckGroupAggroDistance;
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        homePoint = transform.position;
    }

    private void Update()
    {
        if (moveToPoint)
            navMeshAgent.SetDestination(moveToPoint.position);
        else if(!hasFoeCheckedRecently)
            StartCoroutine(CheckForFoes());

        if (moveToPoint && !hasDistanceCheckedRecently)
            StartCoroutine(CheckForDistanceBreak());
        transform.rotation = Quaternion.identity;
    }

    private void AssignAggroTarget(Transform target)
    {
        moveToPoint = target;
        if (moveToPoint != null)
            EnemyEventManager.NewAggro(gameObject, moveToPoint);
        else
            StartCoroutine(GoHome());
    }

    private void CheckGroupAggroDistance(GameObject sender, Transform target)
    {
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

    private IEnumerator CheckForDistanceBreak()
    {
        if (Vector3.Distance(transform.position, moveToPoint.position) > aggroRange * 2f)
            moveToPoint = null;
        hasDistanceCheckedRecently = true;
        yield return new WaitForSeconds(1.5f);
        hasDistanceCheckedRecently = false;
    }

    private IEnumerator CheckForFoes()
    {
        for(int i = 0; i < 72; ++i)
        {
            float angle = i * 5;
            Ray ray = new Ray();
            ray.origin = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            ray.direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
            RaycastHit raycastHit = new RaycastHit();
            Debug.DrawRay(ray.origin, ray.direction * aggroRange, Color.black, .7f);
            if (Physics.Raycast(ray, out raycastHit, aggroRange, layerMask))
            {
                /*if (raycastHit.collider.gameObject.layer == 9)
                {*/
                    Debug.Log(gameObject.name + " sees " + raycastHit.collider.gameObject.name);
                    hitList.Add(raycastHit);
                //}
            }
            else
                moveToPoint = null;
        }

        AssignAggroTarget(DetermineClosestTarget());
        hasFoeCheckedRecently = true;
        yield return new WaitForSeconds(0.7f);
        hasFoeCheckedRecently = false;
    }

    private IEnumerator DelayedAggroAssignment(Transform target)
    {
        yield return new WaitForSeconds(0.35f);
        AssignAggroTarget(target);
    }

    private IEnumerator GoHome()
    {
        yield return new WaitForSeconds(0.35f);
        if (moveToPoint == null)
            navMeshAgent.SetDestination(homePoint);
    }
}
