using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventManager
{
    public delegate void MenuButtonEvent(Object sender);
    public static event MenuButtonEvent OnButtonPress;

    public static void ButtonPress(Object sender)
    {
        if(OnButtonPress != null) OnButtonPress(sender);
    }
}