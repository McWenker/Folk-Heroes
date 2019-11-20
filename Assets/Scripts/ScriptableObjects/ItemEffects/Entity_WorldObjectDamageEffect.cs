using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Entity_WorldObjectDamageEffect : Effect
{
    public int objectDamage;
    public TileEffectTargetStruct[] targets;
    [SerializeField] private Entity_TargetingSolution[] targetSolutions;

    public override void DoEffect(Vector3 effectLocation, LayerMask whatIsTarget, float chargePercent)
    {
        int damage = objectDamage;
        if(chargePercent > 0)
            damage += (int)(chargeGraph.Evaluate(chargePercent)*chargeMultiplier);

        for(int i = 0; i < targetSolutions.Length; ++i)
        {
            if(targetSolutions[i].GetTargets(effectLocation, whatIsTarget) != null)
            {
                foreach(Transform t in targetSolutions[i].GetTargets(effectLocation, whatIsTarget, maxTargets))
                {
                    if(t.GetComponent<WorldObject>() != null)
                    {
                        if(IsValidTarget(t.GetComponent<WorldObject>()) && t.GetComponent<Health>() != null)
                        {
                            t.GetComponent<Health>().Damage(damage);
                        }
                    }                
                }
            }            
        }        
    }

    // this is old function for radius area
    private List<WorldObject> GetTargetTiles(Vector3 location)
    {
        int gridRadius = Mathf.FloorToInt(3);
        List<WorldObject> returnList = new List<WorldObject>();
        
        Vector3Int centerTileLoc = Vector3Int.FloorToInt(location);
        centerTileLoc = new Vector3Int(centerTileLoc.x, 0, centerTileLoc.z); 

        WorldObject tileWorldObject = GridManager.instance.objectTiles[centerTileLoc].WorldObject;

        if(tileWorldObject != null && tileWorldObject.GetComponent<Health>() != null)
            returnList.Add(tileWorldObject);
        
        Vector3Int tileLocation;
        if(gridRadius > 1)
        {
            for(int i = -gridRadius; i <= gridRadius; ++i)
            {
                for(int j = -gridRadius; j <= gridRadius; ++j)
                {
                    if(i == 0 && j == 0)
                    {
                        continue; //already did above
                    }
                    else if((Math.Abs(i) + Mathf.Abs(j)) < gridRadius)
                    {
                        tileLocation = new Vector3Int(centerTileLoc.x + i, 0, centerTileLoc.z + j);
                        tileWorldObject = GridManager.instance.objectTiles[tileLocation].WorldObject;
                        if(tileWorldObject != null && tileWorldObject.GetComponent<Health>() != null)
                            returnList.Add(tileWorldObject);
                    }                
                }
            }
        }
        return returnList;
    }

    private bool IsValidTarget(WorldObject target)
    {
        for(int i = 0; i < target.Targetability.Length; ++i)
        {
            for(int j = 0; j < targets.Length; ++j)
            {
                if(target.Targetability[i].targetType == targets[j].targetType)
                {
                    if(target.Targetability[i].canTarget == true)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/ItemEffects/WorldObjectDamageEffect/Entity_Targeting")]
    public static void CreateWorldObjectDamageEffect()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save World Object Damage Effect", "New World Object Damage Effect", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Entity_WorldObjectDamageEffect>(), path);
    }
    #endif
}
