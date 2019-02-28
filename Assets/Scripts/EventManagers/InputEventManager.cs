using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventManager : MonoBehaviour
{
	public delegate void IdleEventHandler(Object sender);
	public delegate void MoveInputEventHandler(Object sender, Vector3 moveDir);
	public delegate void MouseInputEventHandler(Object sender, int buttonFired);
	public delegate void ControlStateInputEventHandler(Object sender);
	public delegate void AbilityInputEventHandler(Object sender, string ability);

	public static event IdleEventHandler OnIdle;
    public static event MoveInputEventHandler OnMove;
	public static event MouseInputEventHandler OnMouseDown;
	public static event MouseInputEventHandler OnMouseUp;
    public static event MouseInputEventHandler OnFire;
	public static event ControlStateInputEventHandler OnControlStateChange;
    public static event AbilityInputEventHandler OnAbilityUse;

	public static void Move(Object sender, Vector3 moveDir)
    {
        if(OnMove != null && !(moveDir.x == 0 && moveDir.z == 0)) OnMove(sender, moveDir);
		else if(OnIdle != null) OnIdle(sender);
    }

	public static void MouseDown(Object sender, int buttonFired)
	{
		if(OnMouseDown != null) OnMouseDown(sender, buttonFired);
	}

	public static void MouseUp(Object sender, int buttonFired)
	{
		if(OnMouseUp != null) OnMouseUp(sender, buttonFired);
	}

	public static void Fire(Object sender, int buttonFired)
	{
		if(OnFire != null) OnFire(sender, buttonFired);
	}

	public static void ControlStateChange(Object sender)
	{
		if(OnControlStateChange != null) OnControlStateChange(sender);
	}

	public static void AbilityUse(Object sender, string ability)
	{
		if(OnAbilityUse != null) OnAbilityUse(sender, ability);
	}
}
