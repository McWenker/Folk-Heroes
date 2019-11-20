using UnityEngine;

public class PlantObject : WorldObject
{
    PlantPoints points;
    PlantTiers currentTier;
    PlantObject_Animator animator;

    PlantScriptableObject castData;

    int daysAlive;
    int daysStagnant;
    int tierIndex;
    int waterLevel;

    bool isDead;

    public override void TileData(WorldTile thisTile)
    {
        if(thisTile.TileBase == castData.wateredSoil)
            Water(1);
    }

    public void Water(int waterAdded)
    {
        waterLevel += waterAdded;
        Debug.Log("Water level: " + waterLevel, gameObject);
    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<PlantObject_Animator>();
        points = GetComponent<PlantPoints>();
        if(points == null)
            points = gameObject.AddComponent<PlantPoints>();
        castData = (PlantScriptableObject)data;
        TimeEventManager.OnDayEnd += DayChange;
        currentTier = castData.tiers[tierIndex];
        //subscribe DayChange to day-change event
    }

    private void DayChange(Object sender)
    {
        //DEMO
        Debug.Log(waterLevel);
        
        if(waterLevel == 1)
            points.Increase(transform, 1);

        Debug.Log("points: " + points.Value, gameObject);

        if(!isDead && sender.GetType() == typeof(DayController))
        {
            if(IsPlantGrowth(out int extra))
            {
                if(currentTier.overflowStays)
                    points.Set(transform, extra/currentTier.overflowFactor);
                else
                    points.Set(transform, 0);
                if(tierIndex+1 < castData.tiers.Length)
                {
                    ++tierIndex;
                    currentTier = castData.tiers[tierIndex];
                    animator.SpriteChange(castData.tiers[tierIndex].tierSprite);
                    daysStagnant = 0;
                }                
            }
            else if(IsPlantDeath())
            {
                points.Set(transform, 0);
                points.LockOrUnlock(transform, true);
                animator.SpriteChange(castData.tiers[tierIndex].deathSprite);
            }
            else
                ++daysStagnant;
            
            ++daysAlive;
            waterLevel = Mathf.Clamp(--waterLevel, 0, 5);
        }        
    }

    private bool IsPlantGrowth(out int overKill)
    {
        if(points.Value >= castData.tiers[tierIndex].growthThresh)
        {
            overKill = points.Value - currentTier.growthThresh;
            return true;
        }
        overKill = 0;
        return false;
    }

    private bool IsPlantDeath()
    {
        if(points.Value <= castData.tiers[tierIndex].deathThresh || daysStagnant >= castData.tiers[tierIndex].stagnantDaysThresh || daysAlive >= castData.maxLife)
        {
            return true;
        }
        return false;
    }

    private void UpdatePlant()
    {
        currentTier = castData.tiers[tierIndex];
    }
}
