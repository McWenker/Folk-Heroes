using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter_Base : MonoBehaviour
{
    [SerializeField] private SpriteAnimator spriteAnim;

    [SerializeField] private Sprite[] idleSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] idleSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthWestAnimationFrameArray;

    [SerializeField] private Sprite[] walkSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthWestAnimationFrameArray;

    [SerializeField] private float idleFrameRate;
    [SerializeField] private float walkFrameRate;

    public void PlayIdleAnimation()
    {
        Sprite[] anim;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= transform.position.x)
            anim = Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.414214f <= transform.position.z ? idleSouthEastAnimationFrameArray : idleNorthEastAnimationFrameArray;
        else
            anim = Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.414214f <= transform.position.z ? idleSouthWestAnimationFrameArray : idleNorthWestAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, idleFrameRate, false);
    }
    
    public void PlayWalkingAnimation()
    {
        Sprite[] anim;
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= transform.position.x)
            anim = Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.414214f <= transform.position.z ? walkSouthEastAnimationFrameArray : walkNorthEastAnimationFrameArray;
        else
            anim = Camera.main.ScreenToWorldPoint(Input.mousePosition).y * 1.414214f <= transform.position.z ? walkSouthWestAnimationFrameArray : walkNorthWestAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, walkFrameRate, true);
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition).y* 1.414214f + ", " + transform.position.z);
    }
}
