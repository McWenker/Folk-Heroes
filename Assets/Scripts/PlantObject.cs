using UnityEngine;

public class PlantObject : WorldObject
{
    PlantPoints points;
    [SerializeField] PlantScriptableObject data;
    PlantTiers currentTier;
    PlantObject_Animator animator;

    int daysAlive;
    int daysStagnant;
    int tierIndex;
    int waterLevel;

    bool isDead;

    public override void TileData(WorldTile thisTile)
    {
        if(thisTile.TileBase == data.wateredSoil)
            Water(1);
    }

    public void Water(int waterAdded)
    {
        waterLevel += waterAdded;
        Debug.Log("Water level: " + waterLevel, gameObject);
    }

    private void Awake()
    {
        animator = GetComponent<PlantObject_Animator>();
        points = GetComponent<PlantPoints>();
        if(points == null)
            points = gameObject.AddComponent<PlantPoints>();
        TimeEventManager.OnDayEnd += DayChange;
        currentTier = data.tiers[tierIndex];
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
                if(tierIndex+1 < data.tiers.Length)
                {
                    ++tierIndex;
                    currentTier = data.tiers[tierIndex];
                    animator.SpriteChange(data.tiers[tierIndex].tierSprite);
                    daysStagnant = 0;
                }                
            }
            else if(IsPlantDeath())
            {
                points.Set(transform, 0);
                points.LockOrUnlock(transform, true);
                animator.SpriteChange(data.tiers[tierIndex].deathSprite);
            }
            else
                ++daysStagnant;
            
            ++daysAlive;
            waterLevel = Mathf.Clamp(--waterLevel, 0, 5);
        }        
    }

    private bool IsPlantGrowth(out int overKill)
    {
        if(points.Value >= data.tiers[tierIndex].growthThresh)
        {
            overKill = points.Value - currentTier.growthThresh;
            return true;
        }
        overKill = 0;
        return false;
    }

    private bool IsPlantDeath()
    {
        if(points.Value <= data.tiers[tierIndex].deathThresh || daysStagnant >= data.tiers[tierIndex].stagnantDaysThresh || daysAlive >= data.maxLife)
        {
            return true;
        }
        return false;
    }

    private void UpdatePlant()
    {
        currentTier = data.tiers[tierIndex];
    }
}
