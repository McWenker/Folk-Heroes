using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float distCoeff;
    [SerializeField] private float dashDistance = 100f;
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private float dashEffectWidth;

    [SerializeField] LayerMask whoToCollide;

    [SerializeField] private int dashCost;
    private Vector3 lastMoveDirection;

    private PlayerCharacter_Animator playerCharacterAnimator;

    private bool dashCooldown;

    private int layerMask;

    private bool moveHalted;

    public bool MoveHalted { get { return moveHalted; }}

    private void Awake()
    {
        //demo
        playerCharacterAnimator = GetComponent<PlayerCharacter_Animator>();
        InputEventManager.OnMove += Movement;
        InputEventManager.OnAbilityUse += HandleDash;
        AnimationEventManager.OnItemChargeStart += Halt;
        AnimationEventManager.OnItemUseCompletion += UnHalt;
        layerMask = 1 << gameObject.layer;
        layerMask = ~layerMask;
    }

    private void Halt(Object sender, Triggers use, ItemSprites spriteInfo)
    {
        if(transform == (Transform)sender)
        {
            moveHalted = true;
        }
    }

    private void UnHalt(Object sender)
    {
        Debug.Log(sender);
        if(transform == (Transform)sender)
        {
            moveHalted = false;
        }
    }

    private void HandleDash(Object sender, string keyPressed)
    {
        if (keyPressed == "Space")
        {
            if(!dashCooldown /*&& energyPool._Energy >= dashCost*/)
            {
                Vector3 beforeDashPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (lastMoveDirection.x != 0 || lastMoveDirection.z != 0)
                {
                    if (TryMove(lastMoveDirection, dashDistance))
                    {
                        Transform dashEffectTransform = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity).transform;
                        dashEffectTransform.eulerAngles = new Vector3(90f, MathUtil.GetAngleFromVectorFloat(lastMoveDirection));
                        dashEffectTransform.localScale = new Vector3(dashDistance / dashEffectWidth, 1.414214f, 1f);
                        dashCooldown = true;
                        StartCoroutine(DashCooldown());
                        //energyPool.ModifyEnergy(-dashCost);
                    }
                }  
            }          
        }
    }

    private void Movement(Object sender, Vector3 baseMoveDir)
    {
        if(!moveHalted)
        {
            if(TryMove(baseMoveDir, speed*Time.deltaTime*distCoeff))
                playerCharacterAnimator.PlayWalkingAnimation(this, baseMoveDir, false);
            
            else
                playerCharacterAnimator.PlayIdleAnimation(this);
        }

        else
        {            
            if(TryMove(baseMoveDir, (speed*Time.deltaTime*distCoeff)/4))
                playerCharacterAnimator.PlayWalkingAnimation(this, baseMoveDir, true);
            
            else
                playerCharacterAnimator.PlayIdleAnimation(this);
        }
    }

    private bool TryMove(Vector3 baseMoveDir, float distance)
    {
        Vector3 moveDir = baseMoveDir;
        bool canMove = CanMove(moveDir, distance);
        if (!canMove)
        {
            //cannot move diagonally
            moveDir = new Vector3(baseMoveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0f && CanMove(moveDir, distance);
            if (!canMove)
            {
                //cannot move horizontally
                moveDir = new Vector3(0f, 0f, baseMoveDir.z).normalized;
                canMove = CanMove(moveDir, distance);
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

    private bool CanMove(Vector3 dir, float distance)
    {
        Debug.DrawRay(rayPoint.position, dir*distance, Color.red, 0.8f);
        return !Physics.Raycast(rayPoint.position, dir, distance, whoToCollide);
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(0.7f);
        dashCooldown = false;
    }
}
