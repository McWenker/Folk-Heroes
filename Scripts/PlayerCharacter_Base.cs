using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter_Base : MonoBehaviour
{
    [SerializeField] private SpriteAnimator spriteAnim;
    [SerializeField] private Weapon_Base weaponBase;

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

    private LayerMask layerMask;
    private Vector3 mousePointInWorld;

    private void Awake()
    {
        layerMask = LayerMask.NameToLayer("GroundPlane");
    }

    private void GetFacing()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            mousePointInWorld = hit.point;
            weaponBase.RotateWeapon(transform.position, mousePointInWorld);
        }
    }

    public void PlayIdleAnimation()
    {
        Sprite[] anim;

        if (mousePointInWorld.x >= transform.position.x)
            anim = mousePointInWorld.z <= transform.position.z + 1.25f ? idleSouthEastAnimationFrameArray : idleNorthEastAnimationFrameArray;
        else
            anim = mousePointInWorld.z <= transform.position.z + 1.25f ? idleSouthWestAnimationFrameArray : idleNorthWestAnimationFrameArray;

        spriteAnim.PlayAnimation(anim, idleFrameRate, false);
    }
    
    public void PlayWalkingAnimation()
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
