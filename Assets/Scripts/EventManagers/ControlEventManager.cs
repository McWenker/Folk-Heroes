using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControlEventManager
{
    public delegate void ControlStateEventHandler(Object sender, ControlState state);
	public delegate ControlState ControlStateActionEventHandler(GameObject sender, ControlState state);
    public static event ControlStateEventHandler OnControlStateSet;
    public static event ControlStateActionEventHandler OnControlStateSwap;

    public static ControlState ControlStateSwap(GameObject sender, ControlState state)
    {
        if (OnControlStateSwap != null) return OnControlStateSwap(sender, state);
		else return ControlState.Combat;
    }

    public static void ControlStateSet(Object sender, ControlState state)
    {
        if(OnControlStateSwap != null) OnControlStateSet(sender, state);
    }
}
