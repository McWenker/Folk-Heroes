using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter_Base : MonoBehaviour
{
    [SerializeField] private GameObject testObj;
    [SerializeField] private SpriteAnimator spriteAnim;
    [SerializeField] private Weapon_Base right_weaponBase;
    [SerializeField] private Weapon_Base left_weaponBase;

    public Transform rightHand;
    public Transform leftHand;

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
    
    private Vector3 mousePointInWorld;    

    private Dictionary<string, Vector3> handDict = new Dictionary<string, Vector3>()
    {
        {"R_southeast", new Vector3(0.572f, 0.55f, 0f)},
        {"L_southeast", new Vector3(-0.2f, 0.44f, 0f)},
        {"R_northeast", new Vector3(0.399f, 0.715f, 0f)},
        {"L_northeast", new Vector3(-0.42f, 0.579f, 0f)},

        {"R_southwest", new Vector3(-0.79f, 0.63f, 0f)},
        {"L_southwest", new Vector3(0.279f, 0.862f, 0f)},
        {"R_northwest", new Vector3(-0.28f, 0.96f, 0f)},
        {"L_northwest", new Vector3(-0.7f, 0.96f, 0f)}
    };

    private Vector3 rightHandPos;
    private Vector3 leftHandPos;

    private void Awake()
    {
        rightHandPos = rightHand.localPosition;
        leftHandPos = leftHand.localPosition;
        InputEventManager.OnMove += PlayWalkingAnimation;
        InputEventManager.OnIdle += PlayIdleAnimation;
    }

    private void GetFacing()
    {
        mousePointInWorld = RayToGroundUtil.FetchMousePointOnGround(1.2f);
        PlaceHands();
        if(mousePointInWorld != Vector3.zero)
        {
            right_weaponBase.RotateWeapon(rightHand.position, mousePointInWorld);
            left_weaponBase.RotateWeapon(leftHand.position, mousePointInWorld);
        }
    }

    private void PlaceHands()
    {
        if(mousePointInWorld.x >= transform.position.x) // facing east
        {
            if(mousePointInWorld.z <= transform.position.z) // southeast
            {
                rightHand.localPosition = handDict["R_southeast"];
                leftHand.localPosition = handDict["L_southeast"];
            }
            else // northeast
            {
                rightHand.localPosition = handDict["R_northeast"];
                leftHand.localPosition = handDict["L_northeast"];
            }
        }
        else
        {
            if(mousePointInWorld.z <= transform.position.z) // southwest
            {
                rightHand.localPosition = handDict["R_southwest"];
                leftHand.localPosition = handDict["L_southwest"];
            }
            else // northwest
            {
                rightHand.localPosition = handDict["R_northwest"];
                leftHand.localPosition = handDict["L_northwest"];
            }
        }
    }

    void PlayIdleAnimation(Object sender)
    {
        Sprite[] anim;

        if (mousePointInWorld.x >= transform.position.x)
            anim = mousePointInWorld.z <= transform.position.z + 1.25f ? idleSouthEastAnimationFrameArray : idleNorthEastAnimationFrameArray;
        else
            anim = mousePointInWorld.z <= transform.position.z + 1.25f ? idleSouthWestAnimationFrameArray : idleNorthWestAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, idleFrameRate, false);
    }
    
    void PlayWalkingAnimation(Object sender, Vector3 movDir)
    {
        Sprite[] anim;
        if(mousePointInWorld.x >= transform.position.x)
            anim = mousePointInWorld.z <= transform.position.z + 1.25f ? walkSouthEastAnimationFrameArray : walkNorthEastAnimationFrameArray;
        else
            anim = mousePointInWorld.z <= transform.position.z + 1.25f ? walkSouthWestAnimationFrameArray : walkNorthWestAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, walkFrameRate, true);
    }

    public void FixedUpdate()
    {
        GetFacing();
    }
}
