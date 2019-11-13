using UnityEngine;

public class Item : MonoBehaviour, I_Item
{
    protected float timeUntilUse = -1;
    protected int useCount = 0;
    protected int lastMouseButton;
    protected Vector3 useLocation;
    protected Vector3 userPosition;
    protected Vector3 target;

    bool isCharging;

    protected float beginUseTime;

    protected float chargePercent;
    public ItemScriptableObject data;

    public LayerMask whatIsTarget;
    public void Drop()
    {}
    public void PickUp()
    {}
    public void Remove()
    {}

    public void BeginMouse(Transform user, Vector3 targetPoint, Transform handSpot, int useType)
    {
        if(timeUntilUse <= 0 && useType < data.useTypes.Length)
        {
            if(useCount >= data.useTypes[useType].uses.Length) // player has looped the useCount
            {
                useCount = 0;
            }

            if(!data.useTypes[useType].uses[useCount].canBeHeld) // trigger can't be held
            {
                Use(user, targetPoint, handSpot, useType, data.useTypes[useType].uses[useCount], 0f);
            }
            else
            {
                StartCharge(user, useType, data.itemSprites);
            }
        }  
    }

    public void HoldMouse(Transform user, Vector3 targetPoint, Transform handSpot, int useType)
    {
        if(timeUntilUse <= 0 && useType < data.useTypes.Length)
        {
            if(!data.useTypes[useType].uses[useCount].canBeHeld) // trigger can't be held
            {
                Use(user, targetPoint, handSpot, useType, data.useTypes[useType].uses[useCount], 0f);
            }
            else if(!isCharging)
            {
                StartCharge(user, useType, data.itemSprites);
            }
            else if(useType == lastMouseButton && Time.time - beginUseTime >= data.useTypes[useType].uses[useCount].maxHoldTime)
            {
                Use(user, targetPoint, handSpot, useType, data.useTypes[useType].uses[useCount], 1f);
                isCharging = false;
            }            
        }        
    }   

    public void EndMouse(Transform user, Vector3 targetPoint, Transform handSpot, int useType)
    {
        if(timeUntilUse <= 0 && useType < data.useTypes.Length)
        {
            if(useCount < data.useTypes[useType].uses.Length)
            {
                if(data.useTypes[useType].uses[useCount].canBeHeld)
                {
                    if(Time.time - beginUseTime > data.useTypes[useType].uses[useCount].minHoldTime)
                    {
                        Use(user, targetPoint, handSpot, useType, data.useTypes[useType].uses[useCount],
                            data.useTypes[useType].uses[useCount].holdScaling.Evaluate((Time.time - beginUseTime)/data.useTypes[useType].uses[useCount].maxHoldTime));
                        isCharging = false;
                    }
                    else if(data.useTypes[useType].uses[useCount].notEnoughHold.effects.Length > 0)
                    {
                        Use(user, targetPoint, handSpot, useType, data.useTypes[useType].uses[useCount].notEnoughHold, 0f);
                        isCharging = false;
                    }
                    else
                    {
                        isCharging = false;
                        AnimationEventManager.ItemAnimationStop(this, data.itemSprites);
                    }
                }
            } 
        }
               
    }

    private void StartCharge(Transform user, int useType, ItemSprites spriteInfo)
    {
        isCharging = true;
        beginUseTime = Time.time;
        lastMouseButton = useType;
        AnimationEventManager.ItemChargeStart(user, data.useTypes[useType].uses[useCount], data.itemSprites);
    }

    public void Use(Transform user, Vector3 targetPoint, Transform handSpot, int useType, Triggers trigger, float holdFactor)
    {
        AnimationEventManager.ItemUseStart(this);
        /*Debug.DrawRay(userPos, (targetPoint - userPos), Color.gray, 0.4f); 
        Debug.Log("Target Point: " + targetPoint);
        Debug.Log("User Position: " + userPos);
        Debug.Log("Use Vector: " + (targetPoint - userPos));   */
        chargePercent = holdFactor;
        target = targetPoint;
        userPosition = user.position;
        useLocation = Vector3.ClampMagnitude((target - userPosition), trigger.range);
        useLocation = new Vector3(useLocation.x, 0f, useLocation.z);
        /*Debug.DrawRay(userPos, useLocation, Color.black, 0.4f);
        Debug.Log("Clamped Use Vector: " + useLocation);
        Debug.Log("Actual Target Point: " + (userPos + useLocation));*/ 
            
        AnimationEventManager.ItemUse(this, trigger, data.itemSprites);
        timeUntilUse = trigger.cooldown;
        ++useCount;
        if(useCount >= data.useTypes[useType].uses.Length)
            useCount = 0;
        lastMouseButton = useType; 
    }

    public void Trigger(Object sender, Triggers trigger)
    {
        if(sender == this)
        {            
            for(int i = 0; i < trigger.effects.Length; ++i)
            {
                if(trigger.effects[i].GetType() == typeof(ProjectileSpawnEffect))
                {
                    ProjectileSpawnEffect proj = (ProjectileSpawnEffect)trigger.effects[i];
                    proj.SpawnProjectile(transform.position, target, whatIsTarget, chargePercent);
                }
                else
                    trigger.effects[i].DoEffect(userPosition + useLocation, whatIsTarget, chargePercent); //fix this LayerMask hardcoding stuff later
            }
        }
        
    }

    protected void FixedUpdate()
    {
        if(isActiveAndEnabled)
            timeUntilUse -= Time.fixedDeltaTime;
        if(timeUntilUse <= -data.resetTime)
            useCount = 0;
    }    

    protected void Awake()
    {
        AnimationEventManager.OnItemUseTrigger += Trigger;
    }
}
