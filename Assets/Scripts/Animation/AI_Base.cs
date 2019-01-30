using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Base : MonoBehaviour
{
    private IUnit unit;
    [SerializeField] private SpriteAnimator spriteAnim;

    public Vector3 LastMoveDirection { get; private set; }
    [SerializeField] public AI_Faction faction;

    [SerializeField] private Sprite[] idleSouthAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthAnimationFrameArray;

    [SerializeField] private Sprite[] walkSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthWestAnimationFrameArray;

    [SerializeField] private float idleFrameRate;
    [SerializeField] private float walkFrameRate;

    public void PlayIdleAnimation(Vector3 facingDir)
    {
        Sprite[] anim;
        if (facingDir.x == 0)
        {
            if (facingDir.z == 0)
                facingDir = LastMoveDirection;
            else
                facingDir = new Vector3(LastMoveDirection.x, 0f, facingDir.z);
        }

        if (facingDir.z > 0)
        {
            LastMoveDirection = facingDir;
            anim = idleNorthAnimationFrameArray;
        }
        else if(facingDir.z < 0)
        {
            LastMoveDirection = facingDir;
            anim = idleSouthAnimationFrameArray;
        }
        else 
            anim = idleSouthAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, idleFrameRate, true);
    }

    public void PlayWalkingAnimation(Vector3 facingDir)
    {
        Sprite[] anim;

        if (facingDir.x == 0)
        {
            if (facingDir.z == 0)
                facingDir = LastMoveDirection;
            else
                facingDir = new Vector3(LastMoveDirection.x, 0f, facingDir.z);
        }

        if (facingDir.x > 0)
        {
            LastMoveDirection = facingDir;
            anim = facingDir.z <= 0 ? walkSouthEastAnimationFrameArray : walkNorthEastAnimationFrameArray;
        }
        else if (facingDir.x < 0)
        {
            LastMoveDirection = facingDir;
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
