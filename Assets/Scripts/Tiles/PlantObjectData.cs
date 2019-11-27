using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantObjectData : WorldObjectData
{
    PlantScriptableObject castData;
    PlantTiers currentTier;
    public PlantTiers CurrentTier
    {
        get
        {
            return currentTier;
        }
    }
    int tierIndex;
    int plantPoints;
    int PlantPoints
    {
        get
        {
            return plantPoints;
        }
        set
        {
            if(!pointLock)
            {
                plantPoints = value;
            }
        }
    }
    bool pointLock;
    int plantTier;
    int daysAlive;
    int daysStagnant;
    public override void OnDayEnd(int waterLevel)
    {
        if(!isDead)
        {
            Debug.Log(waterLevel + "," + currentTier.idealWaterLevel);        
            plantPoints += CheckWaterLevel(waterLevel);
            Debug.Log(plantPoints);
            if(IsPlantGrowth(out int extra))
            {
                if(currentTier.overflowStays)
                    PlantPoints = extra/currentTier.overflowFactor;
                else
                    PlantPoints = 0;
                if(tierIndex+1 < castData.tiers.Length)
                {
                    ++tierIndex;
                    currentTier = castData.tiers[tierIndex];
                    //animator.SpriteChange(castData.tiers[tierIndex].tierSprite); need to figure this bit out still
                    daysStagnant = 0;
                }                
            }
            else if(IsPlantDeath())
            {
                PlantPoints = 0;
                pointLock = true;
                //animator.SpriteChange(castData.tiers[tierIndex].deathSprite);
            }
            else
                ++daysStagnant;
        }
    }

    public PlantObjectData(WorldObjectScriptableObject defaultData) : base(defaultData)
    {
        castData = defaultData as PlantScriptableObject;
        plantTier = 0;
        currentTier = castData.tiers[0];
    }

    private int CheckWaterLevel(int waterLevel)
    {
        if(waterLevel <= currentTier.idealWaterLevel + currentTier.floodTolerance &&
            waterLevel >= currentTier.idealWaterLevel + currentTier.droughtTolerance) // within range, but not perfect
        {
            if(waterLevel > currentTier.idealWaterLevel) // too high, 1 - (value / max)
            {           
                Debug.Log("too high");     
                return (int)(100 - (Mathf.Abs(currentTier.idealWaterLevel - waterLevel)/Mathf.Abs(currentTier.floodTolerance - currentTier.idealWaterLevel) * castData.wateredSoilGrowth));
            }
            if(waterLevel < currentTier.idealWaterLevel) // too low, 1 - (value abs / max abs)
            {
                Debug.Log("too low");  
                return (int)(100 - (Mathf.Abs(waterLevel - currentTier.idealWaterLevel)/Mathf.Abs(currentTier.droughtTolerance - currentTier.idealWaterLevel) * castData.wateredSoilGrowth));
            }
            else // PERFECT!
            {
                Debug.Log("goldilocks");  
                return 100;
            }
        }
        else // out of range
        {
            if(waterLevel > currentTier.idealWaterLevel) // too high
            {
                Debug.Log("way too high");  
                return -(int)((waterLevel - (currentTier.idealWaterLevel + currentTier.floodTolerance)) * castData.wateredSoilGrowth);
            }
            else // too low
            {
                Debug.Log("way too low");  
                return -(int)((waterLevel - (currentTier.droughtTolerance - currentTier.idealWaterLevel)) * castData.wateredSoilGrowth);
            }
        }
    }

    private bool IsPlantGrowth(out int overKill)
    {
        if(plantPoints >= currentTier.growthThresh)
        {
            overKill = plantPoints - currentTier.growthThresh;
            return true;
        }
        overKill = 0;
        return false;
    }

    private bool IsPlantDeath()
    {
        if(plantPoints <= currentTier.deathThresh || daysStagnant >= currentTier.stagnantDaysThresh || daysAlive >= castData.maxLife)
        {
            return true;
        }
        return false;
    }
}
