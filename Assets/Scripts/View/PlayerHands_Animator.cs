using UnityEngine;

public class PlayerHands_Animator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer leftHandSpriteRenderer;
    [SerializeField] private SpriteRenderer rightHandSpriteRenderer;
    [SerializeField] private float attackFrameRate;
    private SpriteAnimator leftHandAnim;
    private SpriteAnimator rightHandAnim;
    Sprite[] leftHandSprites;
    Sprite[] rightHandSprites; 

    private void ItemSwap(Object sender, Item activeItem)
    {
        if(activeItem.data.itemSprites.leftHandSprite == null)
            leftHandSpriteRenderer.enabled = false;
        else
        {
            leftHandSpriteRenderer.enabled = true;
            leftHandSpriteRenderer.sprite = activeItem.data.itemSprites.leftHandSprite;
        }            
        if(activeItem.data.itemSprites.rightHandSprite == null)
            rightHandSpriteRenderer.enabled = false;
        else
        {
            rightHandSpriteRenderer.enabled = true;
            rightHandSpriteRenderer.sprite = activeItem.data.itemSprites.rightHandSprite;
        }
    }

    // input is negative, to invert rotation caused by aiming pivot
    public void RotateHandSprites(Vector3 eulerAngles)
    {
        leftHandSpriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f - eulerAngles.y));
        rightHandSpriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f - eulerAngles.y));

        if(leftHandSpriteRenderer.transform.position.z  - transform.position.z > 0.1600f)
            leftHandSpriteRenderer.sortingOrder = 0;
        else
            leftHandSpriteRenderer.sortingOrder = 2;
        if(rightHandSpriteRenderer.transform.position.z  - transform.position.z > 0.1600f)
            rightHandSpriteRenderer.sortingOrder = 0;
        else
            rightHandSpriteRenderer.sortingOrder = 2;

    }

    public void FlipHandSprites(bool flipSprites)
    {
        if(!flipSprites)
        {
            leftHandSpriteRenderer.transform.localScale = Vector3.one;
            rightHandSpriteRenderer.transform.localScale = Vector3.one;
        }
        else
        {
            leftHandSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
            rightHandSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void ItemCharge(Object sender, Triggers currentUse, ItemSprites spriteInfo)
    {
        Sprite[] leftAnim;
        Sprite[] rightAnim;

        ItemUses chargeUse = (ItemUses)currentUse;
        float chargeDuration = chargeUse.maxHoldTime - chargeUse.minHoldTime;

        leftAnim = chargeUse.leftHandChargeSprites;
        rightAnim = chargeUse.rightHandChargeSprites;
        
        leftHandAnim.PlayAnimation(leftAnim, chargeDuration/leftAnim.Length, currentUse.keyFrame, (() =>
            {

            }), (() => 
            {
            }));
        rightHandAnim.PlayAnimation(rightAnim, chargeDuration/rightAnim.Length, currentUse.keyFrame, (() =>
            {

            }), (() => 
            {
            }));
    }

    void ItemUseAnimation(Object returnToSender, Triggers currentUse, ItemSprites spriteInfo)
    {
        Sprite[] leftAnim;
        Sprite[] rightAnim;

        leftAnim = currentUse.leftHandSprites;
        rightAnim = currentUse.rightHandSprites;
        
        leftHandAnim.PlayAnimation(leftAnim, attackFrameRate, currentUse.keyFrame, (() =>
            {

            }), (() => 
            {
                leftHandSpriteRenderer.sprite = spriteInfo.leftHandSprite;
            }));
        rightHandAnim.PlayAnimation(rightAnim, attackFrameRate, currentUse.keyFrame, (() =>
            {
                if(spriteInfo.wrapSwing)
                {
                    if(rightHandSpriteRenderer.transform.position.x < transform.position.x && rightHandSpriteRenderer.transform.position.z > transform.position.z)
                        rightHandSpriteRenderer.sortingOrder = 3;
                }

                AnimationEventManager.ItemUseTrigger(returnToSender, currentUse);
            }), (() => 
            {
                rightHandSpriteRenderer.sortingOrder = 1;
                AnimationEventManager.ItemUseCompletion(transform);
                rightHandSpriteRenderer.sprite = spriteInfo.rightHandSprite;
            }));
    }

    void ItemAnimStop(Object sender, ItemSprites spriteInfo)
    {
        leftHandAnim.SetSprite(transform, spriteInfo.leftHandSprite);
        rightHandAnim.SetSprite(transform, spriteInfo.rightHandSprite);
        AnimationEventManager.ItemUseCompletion(transform);
    }

    void Awake()
    {
        leftHandAnim = leftHandSpriteRenderer.GetComponent<SpriteAnimator>();
        rightHandAnim = rightHandSpriteRenderer.GetComponent<SpriteAnimator>();
        InventoryEventManager.OnItemSwap += ItemSwap;
        AnimationEventManager.OnItemChargeStart += ItemCharge;
        AnimationEventManager.OnItemUse += ItemUseAnimation;
        AnimationEventManager.OnItemAnimationStop += ItemAnimStop;
    }
}
