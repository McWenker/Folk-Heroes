using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Base_Enemy : MonoBehaviour
{
    private IAttack unit;
    private Vector3 lastMoveDirection;
    private AI_Base ai_B;
    [SerializeField] private SpriteAnimator spriteAnim;
    [SerializeField] private Sprite[] attackSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] attackSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] attackNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] attackNorthWestAnimationFrameArray;
    [SerializeField] private float attackFrameRate;

    private void Awake()
    {
        ai_B = GetComponent<AI_Base>();
        unit = GetComponent<IAttack>();
    }

    public void PlayAttackAnimation(Vector3 facingDir)
    {
        if (ai_B != null)
            lastMoveDirection = ai_B.LastMoveDirection;

        Sprite[] anim;

        if (facingDir.x == 0)
        {
            if (facingDir.z == 0)
                facingDir = lastMoveDirection;
            else
                facingDir = new Vector3(lastMoveDirection.x, 0f, facingDir.z);
        }
        else if (facingDir.z == 0)
            facingDir = new Vector3(facingDir.x, lastMoveDirection.z);

        if (facingDir.x > 0)
        {
            lastMoveDirection = facingDir;
            anim = facingDir.z <= 0 ? attackSouthEastAnimationFrameArray : attackNorthEastAnimationFrameArray;
        }
        else if (facingDir.x < 0)
        {
            lastMoveDirection = facingDir;
            anim = facingDir.z <= 0 ? attackSouthWestAnimationFrameArray : attackNorthWestAnimationFrameArray;
        }
        else
            anim = attackSouthEastAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, attackFrameRate, false, 1, () =>
        {
            unit.AttackCompleted();
        });
    }

    internal void PlayRecoveryAnimation(object lookAtPosition)
    {
        throw new NotImplementedException();
    }
}
