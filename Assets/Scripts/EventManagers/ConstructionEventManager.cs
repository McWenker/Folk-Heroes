using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructionEventManager
{
	public delegate void BuildActionEventHandler(GameObject sender);
    public static event BuildActionEventHandler OnBuild;
	public static event BuildActionEventHandler OnSpriteFlip;
}
