using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private ControlStateHandler controlStateHandler;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashDistance = 100f;
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private float dashEffectWidth;

    [SerializeField] private int dashCost;
    private Vector3 lastMoveDirection;

    private PlayerCharacter_Base playerCharacterBase;
    private Energy energyPool;

    private bool dashCooldown;

    private Weapon rightWeapon;
    private Weapon leftWeapon;

    private void Awake()
    {
        playerCharacterBase = GetComponent<PlayerCharacter_Base>();
        energyPool = GetComponent<Energy>();
        rightWeapon = playerCharacterBase.rightHand.GetComponentInChildren<Weapon>();
        leftWeapon = playerCharacterBase.leftHand.GetComponentInChildren<Weapon>();

        InputEventManager.OnFire += Fire;
        InputEventManager.OnMove += TryMove;
        InputEventManager.OnAbilityUse += HandleDash;
        EquipmentEventManager.OnEquip += WeaponToggle;
    }

    public void WeaponToggle(Object sender, bool weaponsOut)
    { 
        playerCharacterBase.rightHand.gameObject.SetActive(weaponsOut);
        playerCharacterBase.leftHand.gameObject.SetActive(weaponsOut);
    }

    private void HandleDash(Object sender, string keyPressed)
    {
        if (keyPressed == "Space")
        {
            if(!dashCooldown && energyPool._Energy >= dashCost)
            {
                Vector3 beforeDashPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                if (lastMoveDirection.x != 0 || lastMoveDirection.z != 0)
                {
                    if (TryMove(lastMoveDirection, dashDistance))
                    {
                        Transform dashEffectTransform = Instantiate(dashEffectPrefab, beforeDashPosition, Quaternion.identity).transform;
                        dashEffectTransform.eulerAngles = new Vector3(90f, MathUtil.GetAngleFromVectorFloat(lastMoveDirection));
                        dashEffectTransform.localScale = new Vector3(dashDistance / dashEffectWidth, 1.414214f, 1f);
                        dashCooldown = true;
                        StartCoroutine(DashCooldown());
                        energyPool.ModifyEnergy(-dashCost);
                    }
                }  
            }          
        }
    }

    private void Fire(Object sender, int buttonFired)
    {
        if(buttonFired == 0)
        {            
            if (controlStateHandler._ControlState == ControlState.Combat && rightWeapon != null && rightWeapon.isActiveAndEnabled)
                rightWeapon.Attack();
        }
        else if(buttonFired == 1)
        {
            if (controlStateHandler._ControlState == ControlState.Combat && leftWeapon != null && leftWeapon.isActiveAndEnabled)
                leftWeapon.Attack();
        }
    }

    private bool CanMove(Vector3 dir, float distance)
    {
        return !Physics.Raycast(rayPoint.position, dir, distance);
    }

    private void TryMove(Object sender, Vector3 baseMoveDir)
    {
        TryMove(baseMoveDir, speed*Time.deltaTime);
    }

    private bool TryMove(Vector3 baseMoveDir, float distance)
    {
        Vector3 moveDir = baseMoveDir;
        // distance coefficient is so that the ray makes it beyond player bounds
        // will later need to be custom for each hero
        bool canMove = CanMove(moveDir, distance*6f);
        if (!canMove)
        {
            //cannot move diagonally
            moveDir = new Vector3(baseMoveDir.x, 0f).normalized;
            canMove = moveDir.x != 0f && CanMove(moveDir, distance*6f);
            if (!canMove)
            {
                //cannot move horizontally
                moveDir = new Vector3(0f, 0f, baseMoveDir.y).normalized;
                canMove = CanMove(moveDir, distance*6f);
            }
        }

        if (canMove)
        {
            lastMoveDirection = moveDir;
            transform.position += moveDir * distance;
            return true;
        }

        else
        {
            return false;
        }
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(0.7f);
        dashCooldown = false;
    }
}
