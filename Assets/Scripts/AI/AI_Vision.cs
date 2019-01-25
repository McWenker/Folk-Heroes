using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Vision : MonoBehaviour, IVision
{
	[SerializeField] Transform nearestHostile;
	AI_Faction faction;
	[SerializeField] float visionRange;
    [SerializeField] LayerMask[] layersToHit;
    int layerMask;
    private List<RaycastHit> hitList = new List<RaycastHit>();
    

    private bool hasFoeCheckedRecently;
    private bool hasDistanceCheckedRecently;


	private void Awake()
	{
		faction = GetComponent<AI_Base>().faction;
        if(faction == AI_Faction.Enemy)        
            EnemyEventManager.OnAggro += CheckGroupAggroDistance;
        layerMask = LayerMaskUtil.GetLayers(layersToHit);
	}
    private Transform AssignAggroTarget(Transform target)
    {
        nearestHostile = target;
        if (nearestHostile != null && faction == AI_Faction.Enemy)
            EnemyEventManager.NewAggro(gameObject, nearestHostile);
        return nearestHostile;
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
        
        if (nearestHostile == null)
        {
            if (Vector3.Distance(transform.position, sender.transform.position) < (2 * visionRange / 3))
                StartCoroutine(DelayedAggroAssignment(target));
        }
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
            //Debug.DrawRay(ray.origin, ray.direction * visionRange, Color.black, .75f);
            if (Physics.Raycast(ray, out raycastHit, visionRange, layerMask))
            {
                hitList.Add(raycastHit);
            }
        }

        return AssignAggroTarget(DetermineClosestTarget());
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
        if(nearestHostile == null)
            return true;
        else
        {
            if (Vector3.Distance(transform.position, nearestHostile.position) > visionRange * 2f)
            {
                AssignAggroTarget(null);
                return false;
            }
            return true;
        }
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
    
    public bool DistanceCheck()
    {
        if (!hasDistanceCheckedRecently)
        {
            StartCoroutine(CheckForDistanceBreakCooldown());
            return CheckForDistanceBreak();
        }
        return true;
    }

    public bool DistanceCheck(Vector3 position, float range)
    {
        return (Vector3.Distance(transform.position, position) <= range);
    }

    public Transform SearchForFoes()
    {
        if (!hasFoeCheckedRecently)
        {
            StartCoroutine(CheckForFoesCooldown());
            return CheckForFoes();
        }
        return nearestHostile;
    }
}
