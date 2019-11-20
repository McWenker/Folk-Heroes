using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapScriptableObject : ScriptableObject
{
    public string sceneName;
    private Dictionary<Vector3, WorldTile>[] tiles;

    public Dictionary<Vector3, WorldTile>[] Tiles
    {
        get { return tiles; }
    }
    public Dictionary<Vector3, WorldTile> GroundTiles
    {
        get { return tiles[0]; }
    }
    public Dictionary<Vector3, WorldTile> WallTiles
    {
        get { return tiles[1]; }
    }
    public Dictionary<Vector3, WorldTile> ObjectTiles
    {
        get { return tiles[2]; }
    }
}
