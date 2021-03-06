﻿using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    private Vector3 mousePointInWorld;  
    private Vector3 pointInWorld;  
    private PlayerHands_Animator animator;
    public Transform handTransform;
    public Transform leftHand;
    public Transform rightHand;
    public Item equippedItem;

    bool locked;

    private void GetFacing()
    {
        mousePointInWorld = RayToGroundUtil.FetchMousePointOnGround(0f);
        pointInWorld = new Vector3(mousePointInWorld.x, handTransform.position.y, mousePointInWorld.z);
        if(pointInWorld != Vector3.zero)
        {
           RotateHands(pointInWorld);
        }
    }    

    private void RotateHands(Vector3 pointTowards)
    {
        if(!locked)
        {
            handTransform.LookAt(pointTowards);
            Vector3 handAngle = handTransform.localRotation.eulerAngles;
            leftHand.localRotation = Quaternion.Euler(-handAngle);
            rightHand.localRotation = Quaternion.Euler(-handAngle);


            animator.RotateHandSprites(handTransform.localRotation.eulerAngles);
            animator.FlipHandSprites(pointTowards.x < transform.position.x && equippedItem.data.itemSprites.flipSprites);
        }
        
    }   

    private void BeginMouse(Object sender, int buttonFired)
    {
        if(buttonFired == 1)
        {
            if(MouseOverInteractable())
                return;
        }
        equippedItem.BeginMouse(transform, pointInWorld, rightHand, buttonFired);
    }

    private bool MouseOverInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if(Vector3.Distance(hit.collider.gameObject.transform.position, transform.position) <= 1f)
            {
                if(hit.collider.gameObject.GetComponent<Interactable>() != null)
                {
                    hit.collider.gameObject.GetComponent<Interactable>().Interact();
                    return true;
                }
            }            
        }
        return false;
    }

    private void HoldMouse(Object sender, int buttonFired)
    {
        equippedItem.HoldMouse(transform, pointInWorld, rightHand, buttonFired);
    }

    private void EndMouse(Object sender, int buttonFired)
    {
        equippedItem.EndMouse(transform, pointInWorld, rightHand, buttonFired);
    }

    private void ItemSwap(Object sender, Item activeItem)
    {
        ItemSwap(activeItem);
    }

    private void ItemSwap(Item activeItem)
    {
        equippedItem = activeItem;
    }

    private void Lock(Object sender)
    {
        locked = true;
    }

    private void UnLock(Object sender)
    {
        if(transform == (Transform)sender)
            locked = false;
    }

    private void Awake()
    {        
        InputEventManager.OnMouseDown += BeginMouse;
        InputEventManager.OnMouseHold += HoldMouse;
        InputEventManager.OnMouseUp += EndMouse;
        InventoryEventManager.OnItemSwap += ItemSwap;
        AnimationEventManager.OnItemUseStart += Lock;
        AnimationEventManager.OnItemUseCompletion += UnLock;
        equippedItem = GetComponent<PlayerInventory>().EmptyItem;
        animator = GetComponent<PlayerHands_Animator>();        
    }

    private void FixedUpdate()
    {
        GetFacing();
    } 
}
