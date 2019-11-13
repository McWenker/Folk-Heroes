using UnityEngine;

public class PlayerCharacter_Animator : MonoBehaviour
{
    [SerializeField] private SpriteAnimator spriteAnim;
    [SerializeField] private Sprite[] idleSouthAnimationFrameArray;
    [SerializeField] private Sprite[] idleSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] idleSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] idleNorthWestAnimationFrameArray;
    [SerializeField] private Sprite[] idleEastAnimationFrameArray;
    [SerializeField] private Sprite[] idleWestAnimationFrameArray;
    [SerializeField] private Sprite[] walkSouthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkSouthWestAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthEastAnimationFrameArray;
    [SerializeField] private Sprite[] walkNorthWestAnimationFrameArray;
    [SerializeField] private float idleFrameRate;
    [SerializeField] private float walkFrameRate;
    [SerializeField] private float runFrameRate;

    Vector3 mousePointInWorld;

    void Awake()
    {
        InputEventManager.OnIdle += PlayIdleAnimation;
    }

    public void PlayIdleAnimation(Object sender)
    {
        if(sender == GetComponent<PlayerCharacter>() || sender.GetType() == typeof(InputController)) //second check to be changed at some point, seems inefficient.
        {
            Sprite[] anim;

            if (mousePointInWorld.x >= transform.position.x)
                anim = mousePointInWorld.z <= transform.position.z ? idleSouthEastAnimationFrameArray : idleNorthEastAnimationFrameArray;
            else
                anim = mousePointInWorld.z <= transform.position.z ? idleSouthWestAnimationFrameArray : idleNorthWestAnimationFrameArray;

            spriteAnim.PlayAnimation(anim, idleFrameRate, false);
        }
        
    }
    
    public void PlayWalkingAnimation(PlayerCharacter sender, Vector3 movDir, bool isHalted)
    {
        if(sender == GetComponent<PlayerCharacter>())
        {
            Sprite[] anim;
            if(mousePointInWorld.x >= transform.position.x)
                anim = mousePointInWorld.z <= transform.position.z ? walkSouthEastAnimationFrameArray : walkNorthEastAnimationFrameArray;
            else
                anim = mousePointInWorld.z <= transform.position.z ? walkSouthWestAnimationFrameArray : walkNorthWestAnimationFrameArray;

            spriteAnim.PlayAnimation(anim, isHalted ? walkFrameRate : runFrameRate, true);
        }
    }

    public void FixedUpdate()
    {
        mousePointInWorld = RayToGroundUtil.FetchMousePointOnGround(0f);
    }
}
