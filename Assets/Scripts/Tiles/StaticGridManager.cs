using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticGridManager
{
    private static Dictionary<Vector3, WorldTile> tiles;
    public static Dictionary<Vector3, WorldTile> Tiles
    {
        get { return tiles; }
        set { tiles = value; }
    }
}
