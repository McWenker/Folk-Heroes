using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
	public GridLayout grid;
    public static GridManager instance;
	public static Vector3 TileOffset = new Vector3(0.5f, 0f, 0.5f);
	public Tilemap baseTilemap;
    public Tilemap baseCollideTilemap;
	public Tilemap objectTilemap;

	public Dictionary<Vector3, WorldTile> tiles;

	Vector3Int position;

	public void RevertTiles(Object sender)
	{
		if(sender.GetType() == typeof(DayController))
		{
			foreach(KeyValuePair<Vector3, WorldTile> entry in tiles)
			{
				if(entry.Value.TileBase.GetType() == typeof(TerrainTile))
				{
					TerrainTile toSet = (TerrainTile)entry.Value.TileBase;
					if(toSet.reversionTile != null)
					{
						entry.Value.TilemapMember.SetTileFlags(entry.Value.LocalPlace, TileFlags.None);
						entry.Value.TilemapMember.SetTile(entry.Value.LocalPlace, toSet.reversionTile);
						entry.Value.TileBase = toSet.reversionTile;
						entry.Value.TilemapMember.SetTileFlags(entry.Value.LocalPlace, TileFlags.LockAll);
					}					
				}
			}
		}		
	}

	private void Awake() 
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		tiles = new Dictionary<Vector3, WorldTile>();
		GetWorldTiles();
		TimeEventManager.OnDayEnd += RevertTiles;
		GridEventManager.OnWorldObjectRemove += RemoveObjectInCell;
	}

	// Use this for initialization
	private void GetWorldTiles () 
	{
		GetTilemapTiles(baseTilemap);
		GetTilemapTiles(baseCollideTilemap);		
		GetTilemapTiles(objectTilemap);
	}

	private void GetTilemapTiles(Tilemap tileMap)
	{
		tileMap.CompressBounds();
		Tilemap baseTile = this.baseTilemap;
		foreach (Vector3Int pos in baseTilemap.cellBounds.allPositionsWithin)
		{
			var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

			/*if (!tileMap.HasTile(localPlace){
				continue;
			}*/
			var tile = new WorldTile
			{
				LocalPlace = localPlace,
				WorldLocation = Vector3Int.RoundToInt(baseTile.CellToWorld(localPlace)),
				TileBase = tileMap.GetTile(localPlace),
				TilemapMember = tileMap,
				Name = localPlace.x + "," + localPlace.y,
				WorldObject = GetObjectInCell(Vector3Int.RoundToInt(baseTile.CellToWorld(localPlace)))
			};
			if(instance.tiles.TryGetValue(tile.WorldLocation, out WorldTile tryTile))
				tryTile = tile;
			else
				instance.tiles.Add(tile.WorldLocation, tile);
		}
	}

	private void RemoveObjectInCell(Object sender, Vector3Int position)
	{
		if(sender.GetType() == typeof(WorldObject))
		{
			if(instance.tiles.TryGetValue(position, out WorldTile wTile))
			{
				if(wTile.WorldObject != null)
					wTile.WorldObject = null;
			}
		}		
	}
	private WorldObject GetObjectInCell(Vector3Int position)
    {
		this.position = position;
        int childCount = objectTilemap.transform.childCount;
		//Debug.Log("World - Min: " + min + ", Max: " + max);
        Bounds bounds = new Bounds(position + (new Vector3(1, 0, 1) * 0.5f), Vector3.one);

        for (int i = 0; i < childCount; i++)
        {
            Transform child = objectTilemap.transform.GetChild(i);
            if (bounds.Contains(child.position))
			{
                return child.GetComponent<WorldObject>();

			}
        }
        return null;
    }

	protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(position, Vector3.one);
    }
}


