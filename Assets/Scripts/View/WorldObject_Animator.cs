using UnityEngine;

public class WorldObject_Animator : MonoBehaviour
{
    SpriteAnimator anim;
    [SerializeField] SpriteRenderer shadow;
    [SerializeField] Sprite[] damageFrameArray;
    [SerializeField] Sprite[] deathFrameArray;
    [SerializeField] int shadowTrigger;
    [SerializeField] Sprite postDeathSprite;
    [SerializeField] Sprite[] postDeathFrameArray;
    [SerializeField] float frameRate;
    private bool isDeathAnim;

    private void Awake()
    {
        anim = GetComponentInChildren<SpriteAnimator>();
        AnimationEventManager.OnDamageTaken += Damage;
        AnimationEventManager.OnDeath += Death;
    }

    private void Damage(Object sender)
    {
        if(this != null && sender.GetType() == typeof(Health) && sender == GetComponent<Health>() && !isDeathAnim)
        {
            anim.PlayAnimation(damageFrameArray, frameRate, false);
        }
    }

    private void Death(Object sender)
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

    private void DeathAnimComplete()
    {
        if(this != null && postDeathSprite != null)
            anim.SetSprite(this.transform, postDeathSprite);
    }
}
