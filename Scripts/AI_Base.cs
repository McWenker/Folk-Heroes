using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Base : MonoBehaviour
{
    private Vector3 lastMoveDirection;
    private IUnit unit;
    [SerializeField] private SpriteAnimator spriteAnim;

    [SerializeField] private Sprite[] idleSouthAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthAnimationFrameArray;

    [SerializeField] private Sprite[] walkSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthWestAnimationFrameArray;

    [SerializeField] private Sprite[] mineSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] mineSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] mineNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] mineNorthWestAnimationFrameArray;

    [SerializeField] private float idleFrameRate;
    [SerializeField] private float walkFrameRate;
    [SerializeField] private float mineFrameRate;

    public void PlayIdleAnimation(Vector3 facingDir)
    {
        Sprite[] anim;
        if (facingDir.x == 0)
        {
            if (facingDir.z == 0)
                facingDir = lastMoveDirection;
            else
                facingDir = new Vector3(lastMoveDirection.x, 0f, facingDir.z);
        }

        if (facingDir.z > 0)
        {
            lastMoveDirection = facingDir;
            anim = idleNorthAnimationFrameArray;
        }
        else if(facingDir.z < 0)
        {
            lastMoveDirection = facingDir;
            anim = idleSouthAnimationFrameArray;
        }
        else 
            anim = idleSouthAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, idleFrameRate, false);
    }

    public void PlayMiningAnimation(Vector3 facingDir)
    {
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
            anim = facingDir.z <= 0 ? mineSouthEastAnimationFrameArray : mineNorthEastAnimationFrameArray;
        }
        else if (facingDir.x < 0)
        {
            lastMoveDirection = facingDir;
            anim = facingDir.z <= 0 ? mineSouthWestAnimationFrameArray : mineNorthWestAnimationFrameArray;
        }
        else
            anim = mineSouthEastAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, mineFrameRate, true, 3, () =>
        {
            unit.MiningCompleted();
        });
    }

    public void PlayWalkingAnimation(Vector3 facingDir)
    {
        Sprite[] anim;

        if (facingDir.x == 0)
        {
            if (facingDir.z == 0)
                facingDir = lastMoveDirection;
            else
                facingDir = new Vector3(lastMoveDirection.x, 0f, facingDir.z);
        }

        if (facingDir.x > 0)
        {
            lastMoveDirection = facingDir;
            anim = facingDir.z <= 0 ? walkSouthEastAnimationFrameArray : walkNorthEastAnimationFrameArray;
        }
        else if (facingDir.x < 0)
        {
            lastMoveDirection = facingDir;
            anim = facingDir.z <= 0 ? walkSouthWestAnimationFrameArray : walkNorthWestAnimationFrameArray;
        }
        else
            anim = walkSouthEastAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, walkFrameRate, true);
    }

    private void Awake()
    {
        unit = GetComponent<IUnit>();
    }
}
