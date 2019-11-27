using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMO_DayChangeInteractable : Interactable
{
    public override void Interact()
    {
        TimeEventManager.DayEnd(this);
    }
}
