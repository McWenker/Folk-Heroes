using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Vision : MonoBehaviour, IVision
{
	[SerializeField] Transform nearestHostile;
	AI_Faction faction;
	[SerializeField] float visionRange;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LayerMask[] layerPriorities;
    int layerMask;
    private List<RaycastHit> hitList = new List<RaycastHit>();
    

    private bool hasFoeCheckedRecently;
    private bool hasDistanceCheckedRecently;

    private bool hasCalledToArmsRecently;


	private void Awake()
	{
		faction = GetComponent<AI_Base>().faction;
        if(faction == AI_Faction.Enemy)        
            EnemyEventManager.OnAggro += CheckGroupAggroDistance;
        layerMask = LayerMaskUtil.GetLayer(layersToHit);
	}
    private Transform AssignAggroTarget(Transform target)
    {
        Debug.Log(target);
        nearestHostile = target;
        if (nearestHostile != null && faction == AI_Faction.Enemy && !hasCalledToArmsRecently)
        {
            EnemyEventManager.NewAggro(gameObject, nearestHostile);
            hasCalledToArmsRecently = true;
            StartCoroutine(CallToArmsCooldown());
        }
        this.hitList.Clear();
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

        if(hitList.Count > 1)
            return AssignAggroTarget(DetermineClosestTarget(DeterminePriorityTargets()));
        else if(hitList.Count == 1)
            return AssignAggroTarget(hitList[0].transform);
        else
            return null;


    }

    private List<Transform> DeterminePriorityTargets()
    {
        int highestPriority = 0;
        int currentPriority;
        List<int> priorityScores = new List<int>(hitList.Count);
        List<Transform> priorityTargets = new List<Transform>();
        for(int i = 0; i < hitList.Count; ++i)
        {
            currentPriority = 0;
            for(int j = 0; j < layerPriorities.Length; ++j)
            {
                if(hitList[i].transform != null)
                {
                    if(LayerMaskUtil.CheckLayerMask(layerPriorities[j], hitList[i].transform.gameObject.layer))
                    {
                        if(hitList[i].transform.tag == "Worker" || hitList[i].transform.tag == "Player")
                            currentPriority += (layerPriorities.Length - j)/2;
                        else
                            currentPriority += (layerPriorities.Length - j);
                    }
                }
            }
            priorityScores.Insert(i, currentPriority);
            if(currentPriority > highestPriority)
                highestPriority = currentPriority;
        }

        for(int k = 0; k < hitList.Count; ++k)
        {
            if(priorityScores[k] == (highestPriority))
                priorityTargets.Add(hitList[k].transform);
        }

        return priorityTargets;
    }

    private Transform DetermineClosestTarget(List<Transform> targets)
    {
        Transform closestFoeTransform = null;
        if (targets.Count > 0)
        {
            foreach (Transform t in targets)
            {
                if (closestFoeTransform == null)
                    closestFoeTransform = t;
                else if (Vector3.Distance(transform.position, closestFoeTransform.position) > Vector3.Distance(transform.position, t.position))
                    closestFoeTransform = t;
            }
        }
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
    private IEnumerator CallToArmsCooldown()
    {
        yield return new WaitForSeconds(2f);
        hasCalledToArmsRecently = false;
    }
	private IEnumerator CheckForDistanceBreakCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        hasDistanceCheckedRecently = false;
    }

    private IEnumerator CheckForFoesCooldown()
    {
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
            hasDistanceCheckedRecently = true;
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
            hasFoeCheckedRecently = true;
            StartCoroutine(CheckForFoesCooldown());
            return CheckForFoes();
        }
        return nearestHostile;
    }
}
