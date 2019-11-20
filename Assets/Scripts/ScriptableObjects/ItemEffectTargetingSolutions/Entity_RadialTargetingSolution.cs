using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Entity_RadialTargetingSolution : Entity_TargetingSolution
{
    private LayerMask whatToHit;
    public override List<Transform> GetTargets(Vector3 startLocation)
    {
        List<Transform> returnList = new List<Transform>();
        Collider[] enemiesToDamage = Physics.OverlapSphere(startLocation, size, whatToHit);
        for(int i = 0; i < enemiesToDamage.Length; ++i)
        {
            returnList.Add(enemiesToDamage[i].transform);
        }
        return returnList;
    }

    public override List<Transform> GetTargets(Vector3 startLocation, LayerMask whoToHit)
    {
        whatToHit = whoToHit;
        return GetTargets(startLocation);
    }

    public override List<Transform> GetTargets(Vector3 startLocation, LayerMask whoToHit, int maxTargets)
    {
        whatToHit = whoToHit;
        List<Transform> returnList = new List<Transform>();
        Collider[] enemiesToDamage = Physics.OverlapSphere(startLocation, size, whatToHit);
        if(maxTargets > enemiesToDamage.Length)
            maxTargets = enemiesToDamage.Length;
        for(int i = 0; i < maxTargets; ++i)
        {
            returnList.Add(enemiesToDamage[i].transform);
        }
        return returnList;
    }
}
