using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEventManager
{
    public delegate void GridChangeEvent(Object sender, Vector3Int gridLoc);
    public static event GridChangeEvent OnWorldObjectRemove;

    public static void WorldObjectRemove(Object sender, Vector3Int gridLoc)
    {
        if(OnWorldObjectRemove != null) OnWorldObjectRemove(sender, gridLoc);
    }
}
