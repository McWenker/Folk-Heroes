using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class SceneGrid
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

    public SceneGrid(string sceneName, Dictionary<Vector3, WorldTile>[] tiles)
    {
        this.sceneName = sceneName;
        this.tiles = new Dictionary<Vector3, WorldTile>[3];
        this.tiles[0] = tiles[0];
        this.tiles[1] = tiles[1];
        this.tiles[2] = tiles[2];
    }

    public void Update(Dictionary<Vector3, WorldTile>[] tiles)
    {
        this.tiles[0] = tiles[0];
        this.tiles[1] = tiles[1];
        this.tiles[2] = tiles[2];
        foreach(KeyValuePair<Vector3, WorldTile> kvp in tiles[2])
        {
            Debug.Log(kvp.Key + ": " + kvp.Value.WorldLocation + ", " + kvp.Value.WorldObject + ", " + kvp.Value.DefaultWorldObjectData);
        }
    }
}
