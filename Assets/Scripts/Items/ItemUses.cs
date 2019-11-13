using UnityEngine;

[System.Serializable]
public class ItemUses : Triggers
{
    public bool canBeHeld;
    public AnimationCurve holdScaling;
    public Sprite[] leftHandChargeSprites;
    public Sprite[] rightHandChargeSprites;
    public float minHoldTime;
    public float maxHoldTime;
    [SerializeField] public Cancels notEnoughHold;
}

