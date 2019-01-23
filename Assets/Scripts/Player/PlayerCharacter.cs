﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float dashDistance = 100f;
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private float dashEffectWidth;
    private Vector3 lastMoveDirection;
    private PlayerCharacter_Base playerCharacterBase;

    private Weapon rightWeapon;
    private Weapon leftWeapon;
    private ControlState controlState;
    private bool controlStateCooldown;
    private bool constructionFlipCooldown;

    private void Awake()
    {
        playerCharacterBase = GetComponent<PlayerCharacter_Base>();
        rightWeapon = playerCharacterBase.rightHand.GetComponentInChildren<Weapon>();
        leftWeapon = playerCharacterBase.leftHand.GetComponentInChildren<Weapon>();
        controlState = ControlEventManager.ControlStateSwap(gameObject, ControlState.Construction);
        ControlEventManager.OnControlStateSet += SetControlState;
    }    

    private void FixedUpdate()
    {
        HandleControlState();
        HandleMovement();
        HandleDash();
        HandleMouse();
        HandleConstructionFlip();
    }

    private void HandleControlState()
    {
        if(Input.GetKey(KeyCode.LeftAlt) && !controlStateCooldown)
        {
            controlState = ControlEventManager.ControlStateSwap(gameObject, controlState);
            controlStateCooldown = true;
            StartCoroutine(ControlStateCooldown());
        }
        playerCharacterBase.rightHand.gameObject.SetActive(controlState == ControlState.Combat);
        playerCharacterBase.leftHand.gameObject.SetActive(controlState == ControlState.Combat);
    }

    private void SetControlState(Object sender, ControlState state)
    {
        if(sender != this)
        {
            SetControlState(state);
        }
    }

    private void SetControlState(ControlState state)
    {
        ControlEventManager.ControlStateSet(this, state);
        controlState = state;   
        playerCharacterBase.rightHand.gameObject.SetActive(controlState == ControlState.Combat);
        playerCharacterBase.leftHand.gameObject.SetActive(controlState == ControlState.Combat);
    }

    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        bool isIdle = moveX == 0 && moveY == 0;
        if(isIdle)
        {
            playerCharacterBase.PlayIdleAnimation();
        }

        else
        {
            Vector3 baseMoveDir = new Vector3(moveX, 0, moveY).normalized;

            if (TryMove(baseMoveDir, speed * Time.deltaTime))
                playerCharacterBase.PlayWalkingAnimation();
            else
                playerCharacterBase.PlayIdleAnimation();
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 beforeDashPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            if (lastMoveDirection.x != 0 || lastMoveDirection.z != 0)
            {
                if (TryMove(lastMoveDirection, dashDistance))
                {
                    Transform dashEffectTransform = Instantiate(dashEffectPrefab, beforeDashPosition, Quaternion.identity).transform;
                    dashEffectTransform.eulerAngles = new Vector3(90f, UtilsClass.GetAngleFromVectorFloat(lastMoveDirection));
                    dashEffectTransform.localScale = new Vector3(dashDistance / dashEffectWidth, 1.414214f, 1f);
                }
            }            
        }
    }

    private void HandleMouse()
    {
        if(Input.GetMouseButton(0))
        {            
            if (controlState == ControlState.Combat && rightWeapon != null && rightWeapon.isActiveAndEnabled)
                rightWeapon.Attack();
            else if(controlState == ControlState.Construction)
            {
                if (!EventSystem.current.IsPointerOverGameObject ())
                    ConstructionEventManager.NewBuild(gameObject);
            }
        }
        else if(Input.GetMouseButton(1))
        {
            if (controlState == ControlState.Combat && leftWeapon != null && leftWeapon.isActiveAndEnabled)
                leftWeapon.Attack();
            else
                SetControlState(ControlState.Combat);
        }
    }

    private void HandleConstructionFlip()
    {
        if(controlState != ControlState.Construction)
            return;
        if((Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) && !constructionFlipCooldown)
        {
            ConstructionEventManager.FlipSprite(gameObject);
            constructionFlipCooldown = true;
            StartCoroutine(ConstructionFlipCooldown());
        }
    }

    private bool CanMove(Vector3 dir, float distance)
    {
        Debug.DrawRay(rayPoint.position, dir, Color.red, 2f);
        return !Physics.Raycast(rayPoint.position, dir, distance);
    }

    private bool TryMove(Vector3 baseMoveDir, float distance)
    {
        Vector3 moveDir = baseMoveDir;
        bool canMove = CanMove(moveDir, distance*3f);
        if (!canMove)
        {
            //cannot move diagonally
            moveDir = new Vector3(baseMoveDir.x, 0f).normalized;
            canMove = moveDir.x != 0f && CanMove(moveDir, distance);
            if (!canMove)
            {
                //cannot move horizontally
                moveDir = new Vector3(0f, 0f, baseMoveDir.y).normalized;
                canMove = CanMove(moveDir, speed * distance);
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

    private IEnumerator ControlStateCooldown()
    {
        yield return new WaitForSeconds(1f);
        controlStateCooldown = false;
    }

    private IEnumerator ConstructionFlipCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        constructionFlipCooldown = false;
    }

}
