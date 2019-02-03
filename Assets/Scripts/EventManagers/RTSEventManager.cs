using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RTSEventManager
{
	public delegate void SelectionMouseDownEventHandler(PlayerCharacter sender);
	public delegate void SelectionMouseUpEventHandler(PlayerCharacter sender);

	public static event SelectionMouseDownEventHandler OnSelectionMouseDown;
	
	public static event SelectionMouseUpEventHandler OnSelectionMouseUp;

	public static void SelectionMouseDown(PlayerCharacter sender)
    {
        if (OnSelectionMouseDown != null) OnSelectionMouseDown(sender);
    }

    public static void SelectionMouseUp(PlayerCharacter sender)
    {
        if(OnSelectionMouseUp != null) OnSelectionMouseUp(sender);
    }
}
