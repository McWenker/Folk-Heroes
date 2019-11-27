using UnityEngine;

public class WorldObject_Animator : MonoBehaviour
{
    protected SpriteAnimator anim;
    protected SpriteOutline outline;
    [SerializeField] protected SpriteRenderer shadow;
    [SerializeField] protected Sprite[] damageFrameArray;
    [SerializeField] protected Sprite[] deathFrameArray;
    [SerializeField] protected int shadowTrigger;
    [SerializeField] protected Sprite postDeathSprite;
    [SerializeField] protected Sprite[] postDeathFrameArray;
    [SerializeField] protected float frameRate;
    protected bool isDeathAnim;

    public void GetData(Sprite[] damageFrameArray, Sprite[] deathFrameArray, int shadowTrigger, Sprite postDeathSprite, Sprite[] postDeathFrameArray, float frameRate, Vector3[] shadow, Color outlineColor)
    {
        this.damageFrameArray = damageFrameArray;
        this.deathFrameArray = deathFrameArray;
        this.shadowTrigger = shadowTrigger;
        this.postDeathSprite = postDeathSprite;
        this.postDeathFrameArray = postDeathFrameArray;
        this.frameRate = frameRate;
        if(shadow.Length > 0)
        {
            this.shadow.transform.position = shadow[0];
            this.shadow.transform.localScale = shadow[1];
        }
        else if(this.shadow != null)   
        {
            this.shadow.enabled = false;
        }
        outline.SetOutline(outlineColor);
    }

    public void SetSprite(Sprite sprite)
    {
        anim.SetSprite(transform, sprite);
    }

    protected void Awake()
    {
        anim = GetComponentInChildren<SpriteAnimator>();
        outline = anim.GetComponent<SpriteOutline>();
        shadow = transform.Find("Shadow") != null ? transform.Find("Shadow").GetComponent<SpriteRenderer>() : null;
        AnimationEventManager.OnDamageTaken += Damage;
        AnimationEventManager.OnDeath += Death;
    }

    protected void Damage(Object sender)
    {
        if(this != null && sender.GetType() == typeof(Health) && sender == GetComponent<Health>() && !isDeathAnim)
        {
            anim.PlayAnimation(damageFrameArray, frameRate, false);
        }
    }

    protected void Death(Object sender)
    {
        if(this != null && sender.GetType() == typeof(Health) && sender == GetComponent<Health>())
        {
            Sprite[] arrayToPlay;
            WorldObject wObj = GetComponent<WorldObject>();
            if(wObj.IsPostDeath || (isDeathAnim && !wObj.IsPostDeath))
                arrayToPlay = postDeathFrameArray;
            else
                arrayToPlay = deathFrameArray;

            if(!isDeathAnim || wObj.IsPostDeath)
            {
                if(!isDeathAnim)
                    isDeathAnim = true;
                anim.PlayAnimation(arrayToPlay, frameRate, shadowTrigger, () =>
                {
                    if(shadow != null && shadow.enabled)
                        shadow.enabled = false;
                }, (() =>
                {
                    isDeathAnim = false;
                    AnimationEventManager.DeathAnimComplete(this.gameObject);                
                    DeathAnimComplete();
                }));
            }         
        }        
    }

    protected void DeathAnimComplete()
    {
        if(this != null && postDeathSprite != null)
            anim.SetSprite(this.transform, postDeathSprite);
    }
}
