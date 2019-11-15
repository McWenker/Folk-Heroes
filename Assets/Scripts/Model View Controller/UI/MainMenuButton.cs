using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MenuButton, IPointerEnterHandler, IPointerExitHandler
{   
    UI_ImageAnimator animator;
    void Awake()
    {
        animator = GetComponent<UI_ImageAnimator>();
    }
    public override void PressButton()
    {
        MenuEventManager.ButtonPress(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.PlayAnimation(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.StopPlaying();
    }
}
