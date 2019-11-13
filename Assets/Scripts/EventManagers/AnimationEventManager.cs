using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    #region ItemEvents
    public delegate void ItemAnimationEvent(Object sender, Triggers trig, ItemSprites spriteInfo);
    public delegate void ItemTriggerEvent(Object sender, Triggers trig);
    public delegate void ToSpriteEvent(Object sender, ItemSprites spriteInfo);
    public delegate void ItemTimeEvent(Object sender);
    public static event ItemAnimationEvent OnItemChargeStart;
    public static event ItemTimeEvent OnItemUseStart;
    public static event ItemAnimationEvent OnItemUse;
    public static event ItemTriggerEvent OnItemUseTrigger;
    public static event ItemTimeEvent OnItemUseCompletion;
    public static event ToSpriteEvent OnItemAnimationStop;

    public static void ItemChargeStart(Object sender, Triggers trig, ItemSprites spriteInfo)
    {
        if(OnItemChargeStart != null) OnItemChargeStart(sender, trig, spriteInfo);
    }
    public static void ItemUseStart(Object sender)
    {
        if(OnItemUseStart != null) OnItemUseStart(sender);
    }
    public static void ItemUse(Object sender, Triggers trig, ItemSprites spriteInfo)
    {
        if(OnItemUse != null) OnItemUse(sender, trig, spriteInfo);
    }

    public static void ItemUseTrigger(Object sender, Triggers trig)
    {
        if(OnItemUseTrigger != null) OnItemUseTrigger(sender, trig);
    }

    public static void ItemUseCompletion(Object sender)
    {
        if(OnItemUseCompletion != null) OnItemUseCompletion(sender);
    }

    public static void ItemAnimationStop(Object sender, ItemSprites spriteInfo)
    {
        if(OnItemAnimationStop != null) OnItemAnimationStop(sender, spriteInfo);
    }
    #endregion
    #region Damage and Death
    public delegate void DamageEvent(Object sender);
    public delegate void DeathEvent(Object sender);
    public static event DamageEvent OnDamageTaken;
    public static event DeathEvent OnDeath;
    public static event DeathEvent OnDeathAnimComplete;
    public static void DamageTaken(Object sender)
    {
        if(OnDamageTaken != null) OnDamageTaken(sender);
    }
    public static void Death(Object sender)
    {
        if(OnDeath != null) OnDeath(sender);
    }
    public static void DeathAnimComplete(Object sender)
    {
        if(OnDeathAnimComplete != null) OnDeathAnimComplete(sender);
    }
    #endregion    

    public delegate void FloatingTextEvent(Object sender, string text);
    public static event FloatingTextEvent OnFloatingText;

    public static void FloatingText(Object sender, string text)
    {
        if(OnFloatingText != null) OnFloatingText(sender, text);
    }
}
