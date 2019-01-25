using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Base_Gatherer : MonoBehaviour
{
    private IGather unit;
    private Vector3 lastMoveDirection;
    private AI_Base ai_B;
    [SerializeField] private SpriteAnimator spriteAnim;
    [SerializeField] private Sprite[] mineSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] mineSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] mineNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] mineNorthWestAnimationFrameArray;
    [SerializeField] private float mineFrameRate;

    private void Awake()
    {
        ai_B = GetComponent<AI_Base>();
        unit = GetComponent<IGather>();
    }

    public void PlayMiningAnimation(Vector3 facingDir)
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
}
