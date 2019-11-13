using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileScriptableObject data;
    private Projectile_Animator animator;
    private LayerMask whoIsTarget;
    private bool isFlying;
    private bool hasHit;
    private float aliveTime;
    private float chargePercent;
    private Vector3 flightDirection;

    public void Fly(LayerMask targetLayer, Vector3 target, float charge)
    {
        whoIsTarget = targetLayer;
        chargePercent = charge;
        flightDirection = target - transform.position;
        flightDirection = Vector3.ClampMagnitude(new Vector3(flightDirection.x, 0f, flightDirection.z), 1f);
        flightDirection = Vector3.ClampMagnitude(new Vector3(flightDirection.x + Random.Range(-data.sway, data.sway), 0, flightDirection.z + Random.Range(-data.sway, data.sway)), 1f);
        transform.LookAt(target);
        animator.BeginAnim(data.defaultSprites);

        aliveTime = 0;
        isFlying = true;
    }

    public void OnTriggerStay(Collider hit)
    {        
        if((((1<<hit.gameObject.layer) & data.whoToIgnore) == 0) && !hasHit)//it was not in an ignore layer and the projectile has not hit anything yet
        {
            hasHit = true;
            for(int i = 0; i < data.effects.Length; ++i)
            {
                Debug.Log("Hit! Not an ignore layer, " + hit.gameObject.name + ", " + hit.transform.position, hit.transform);
                data.effects[i].DoEffect(hit.transform.position, whoIsTarget, chargePercent);
            }
            Dud();
        } 
            
    }

    private void Awake()
    {
        animator = GetComponent<Projectile_Animator>();
    }

    private void FixedUpdate()
    {
        if(isFlying && aliveTime <= data.maxFlightDuration)
        {
            transform.position += flightDirection * (data.speed + (data.chargeGraph.Evaluate(chargePercent) * data.chargeMultiplier)) * Time.fixedDeltaTime;
            aliveTime += Time.fixedDeltaTime;
        }
        else if (aliveTime > data.maxFlightDuration)
        {
            Dud();
        }
    }

    private void Dud()
    {
        isFlying = false;
        // dud animation, fall to ground, etc
        animator.DudAnim(data.dudSprites);
    }
}
