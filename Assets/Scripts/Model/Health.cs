using UnityEngine;

public class Health : Resource
{
    private float damageCooldown;
    public void Damage(int damageToTake)
    {
        if(damageCooldown <= 0)
        {
            // animation, particles, etc
            // damage events here
            Decrease(damageToTake);
            //Debug.Log("Ouch! " + gameObject.name + " took " + damageToTake + " damage, and has " + currentValue + " health left.");
            if(GetComponent<PlayerCharacter_Animator>() != null || GetComponent<NPC_Animator>() != null)
                AnimationEventManager.FloatingText(this, damageToTake.ToString());
            
            // check for death, etc
            if(currentValue <= 0)
                AnimationEventManager.Death(this);
            else
                AnimationEventManager.DamageTaken(this);
            damageCooldown = 0.4f;      
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        damageCooldown -= Time.fixedDeltaTime;
    }

    private void Awake()
    {
        minValue = 0;
        currentValue = maxValue;
    }
}
