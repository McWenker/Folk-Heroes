using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Weapon weapon;

    private void Awake()
    {
        playerCharacterBase = GetComponent<PlayerCharacter_Base>();
        weapon = GetComponentInChildren<Weapon>();
    }    

    private void FixedUpdate()
    {
        HandleMovement();
        HandleDash();
        HandleAttack();
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

    private void HandleAttack()
    {
        if(Input.GetMouseButton(0))
        {
            if (weapon != null)
                weapon.Attack();
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
        bool canMove = CanMove(moveDir, distance);
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

}
