using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
    [SerializeField] private SpriteAnimator spriteAnim;
    [SerializeField] private Transform attackRing;

    [SerializeField] private Sprite[] idleEastAnimationArray;
    [SerializeField] private Sprite[] idleWestAnimationArray;

    [SerializeField] private Sprite[] attackEastAnimationArray;
    [SerializeField] private Sprite[] attackWestAnimationArray;

    [SerializeField] private float idleFrameRate;
    [SerializeField] private float attackFrameRate;
    [SerializeField] private int attackTriggerFrame;

    private Vector3 spriteAngle;
    private Vector3 attackAngle;
    private bool facingEast;

    public void PlayIdleAnimation()
    {
        Sprite[] anim;

        if (facingEast)
            anim = idleEastAnimationArray;
        else
            anim = idleWestAnimationArray;

        spriteAnim.PlayAnimation(anim, idleFrameRate, false);
    }

    public void PlayAttackAnimation(Action onTrigger, Action onAnimationCompleted)
    {
        Sprite[] anim;

        if (facingEast)
            anim = attackEastAnimationArray;
        else
            anim = attackWestAnimationArray;

        spriteAnim.PlayAnimation(anim, attackFrameRate, attackTriggerFrame, () =>
        {
            onTrigger();
        }, () => 
        {
            onAnimationCompleted();
        });
    }

    public void RotateWeapon(Vector3 charPosition, Vector3 pointToward)
    {
        // Rotate Object based on facing
        if (pointToward.x > charPosition.x)
            facingEast = true;
        else
            facingEast = false;

        // 2.5D camera trix
        if (pointToward.z - charPosition.z > 1.25)
            spriteAnim.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f);
        else
            spriteAnim.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.4f);

        float AngleRad;
        //AngleRad = Mathf.Atan2(pointToward.z - charPosition.z, pointToward.x - charPosition.x);
        AngleRad = Mathf.Atan2(pointToward.z - charPosition.z, pointToward.x - charPosition.x);

        // Get Angle in Degrees
        float AngleDeg = AngleRad * Mathf.Rad2Deg;
        
        spriteAngle = new Vector3(0, 0, AngleDeg);
        attackAngle = new Vector3(0, -AngleDeg, 0);
        spriteAnim.transform.localRotation = Quaternion.Euler(spriteAngle);
        attackRing.rotation = Quaternion.Euler(attackAngle);
    }
}
