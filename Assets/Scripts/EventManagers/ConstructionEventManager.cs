using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructionEventManager
{
	public delegate void BuildActionEventHandler(GameObject sender);
    public static event BuildActionEventHandler OnBuild;
	public static event BuildActionEventHandler OnSpriteFlip;

    public static void NewBuild(GameObject sender)
    {
        if (OnBuild != null) OnBuild(sender);
    }

	public static void FlipSprite(GameObject sender)
	{
		if(OnSpriteFlip != null) OnSpriteFlip(sender);
	}
}
