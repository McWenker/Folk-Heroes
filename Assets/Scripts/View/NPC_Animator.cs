using System.Collections;
using UnityEngine;

public class NPC_Animator : MonoBehaviour
{
    // very barebones for now
    SpriteOutline spriteOutline;
    SpriteRenderer spriteRenderer;
    SpriteAnimator spriteAnimator;
    Color startingColor;
    Color startingOutline;
    Health thisNPCHealth;
    [SerializeField] ParticleSystem damageParticle;
    [SerializeField] Transform groundPlane;

    void Awake()
    {
        AnimationEventManager.OnDamageTaken += Damage;
        spriteOutline = GetComponentInChildren<SpriteOutline>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteAnimator = GetComponentInChildren<SpriteAnimator>();
        thisNPCHealth = GetComponent<Health>();
        startingColor = spriteRenderer.color;
        startingOutline = spriteOutline.outlineColor;
        damageParticle.collision.SetPlane(0, groundPlane);
    }

    void Damage(Object sender)
    {
        if(thisNPCHealth != null && sender.GetType() == typeof(Health))
        {
            if(thisNPCHealth == (Health)sender)
            {
                startingColor = spriteRenderer.color;
                damageParticle.Play();
                StartCoroutine(DamageFlash());
            }
        }
    }

    IEnumerator DamageFlash()
    {
        float whenAreWeDone = Time.time + 0.5f;
        while(Time.time < whenAreWeDone){
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = (spriteRenderer.color == startingColor) ? Color.red : startingColor;

        }
        spriteRenderer.color = startingColor;
    }
}
