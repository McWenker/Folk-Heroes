using UnityEngine;

public class InputEventManager : MonoBehaviour
{
	public delegate void IdleEventHandler(Object sender);
	public delegate void MoveInputEventHandler(Object sender, Vector3 moveDir);
	public delegate void NumericOrMouseInputEventHandler(Object sender, int buttonFired);
	public delegate void ControlStateInputEventHandler(Object sender);
	public delegate void AbilityInputEventHandler(Object sender, string ability);
	public delegate void MouseScrollInputEventHandler(Object sender, float direction);

	public static event IdleEventHandler OnIdle;
    public static event MoveInputEventHandler OnMove;
	public static event NumericOrMouseInputEventHandler OnMouseDown;
    public static event NumericOrMouseInputEventHandler OnMouseHold;
	public static event NumericOrMouseInputEventHandler OnMouseUp;
	public static event ControlStateInputEventHandler OnControlStateChange;
    public static event AbilityInputEventHandler OnAbilityUse;
	public static event NumericOrMouseInputEventHandler OnActionBarPress;
	public static event MouseScrollInputEventHandler OnScrollWheel;

	public static void Move(Object sender, Vector3 moveDir)
    {
        if(OnMove != null && !(moveDir.x == 0 && moveDir.z == 0)) OnMove(sender, moveDir);
		else if(OnIdle != null) OnIdle(sender);
    }

	public static void MouseDown(Object sender, int buttonFired)
	{
		if(OnMouseDown != null) OnMouseDown(sender, buttonFired);
	}

	public static void MouseHold(Object sender, int buttonFired)
	{
		if(OnMouseHold != null) OnMouseHold(sender, buttonFired);
	}

	public static void MouseUp(Object sender, int buttonFired)
	{
		if(OnMouseUp != null) OnMouseUp(sender, buttonFired);
	}	

	public static void MouseScroll(Object sender, float direction)
	{
		if(OnScrollWheel != null) OnScrollWheel(sender, direction);
	}

	public static void ControlStateChange(Object sender)
	{
		if(OnControlStateChange != null) OnControlStateChange(sender);
	}

	public static void AbilityUse(Object sender, string ability)
	{
		if(OnAbilityUse != null) OnAbilityUse(sender, ability);
	}

	public static void ActionBarPress(Object sender, int button)
	{
		if(OnActionBarPress != null) OnActionBarPress(sender, button);
	}
}
