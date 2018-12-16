using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter_Base : MonoBehaviour
{
    [SerializeField] private GameObject testObj;
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
    
    private Vector3 mousePointInWorld;    

    private void GetFacing()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane hPlane = new Plane(Vector3.up, new Vector3(0, 1.2f, 0));
        float distance = 0;

        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out distance))
        {
            // get the hit point:
            mousePointInWorld = ray.GetPoint(distance);
            weaponBase.RotateWeapon(transform.position, mousePointInWorld);
        }
    }

    public void CreateTargetTestObject()
    {
        GameObject testBlock = Instantiate(testObj, mousePointInWorld, Quaternion.identity);
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
