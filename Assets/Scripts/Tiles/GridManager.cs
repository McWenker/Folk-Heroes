using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
	public GridLayout grid;
	GameObject emptyWorldObjectPrefab;
    public static GridManager instance;
	public static Vector3 TileOffset = new Vector3(0.5f, 0f, 0.5f);
	public Tilemap groundTilemap;
    public Tilemap wallTilemap;
	public Tilemap objectTilemap;

	public Dictionary<Vector3, WorldTile> groundTiles;
	public Dictionary<Vector3, WorldTile> wallTiles;
	public Dictionary<Vector3, WorldTile> objectTiles;

	Vector3Int position;

	public void WorldObjectPrefab(GameObject emptyWorldObjectPrefab)
	{
		this.emptyWorldObjectPrefab = emptyWorldObjectPrefab;
	}

	public void InitSceneGrid(string sceneName)
	{
		instance.grid = FindObjectOfType<Grid>().gameObject.GetComponent<GridLayout>();
		instance.groundTilemap = instance.grid.transform.Find("Tilemap_Ground").GetComponent<Tilemap>();
		instance.wallTilemap = instance.grid.transform.Find("Tilemap_Wall").GetComponent<Tilemap>();
		instance.objectTilemap = instance.grid.transform.Find("Tilemap_Object").GetComponent<Tilemap>();
		if(StaticGridManager.CheckScene(sceneName))
		{
			instance.groundTiles = StaticGridManager.GetSceneDict(sceneName)[0];
			instance.wallTiles = StaticGridManager.GetSceneDict(sceneName)[1];
			instance.objectTiles = StaticGridManager.GetSceneDict(sceneName)[2];
			UpdateWorldTiles();
		}
		else
		{
			instance.groundTiles = new Dictionary<Vector3, WorldTile>();
			instance.wallTiles = new Dictionary<Vector3, WorldTile>();
			instance.objectTiles = new Dictionary<Vector3, WorldTile>();
			GetWorldTiles();
			Dictionary<Vector3, WorldTile>[] tileGrids = { instance.groundTiles, instance.wallTiles, instance.objectTiles };
			StaticGridManager.AddScene(sceneName, tileGrids);
		}		
	}

	private void SaveGrid(Object sender, int index, string dest)
	{
		Dictionary<Vector3, WorldTile>[] tileGrids = { instance.groundTiles, instance.wallTiles, instance.objectTiles };
		StaticGridManager.UpdateScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(index).name, tileGrids);
	}

	private void RevertTiles(Object sender)
	{
		if(sender.GetType() == typeof(DayController))
		{
			foreach(KeyValuePair<Vector3, WorldTile> entry in instance.groundTiles)
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
		GameplayEventManager.OnSceneChange += SaveGrid;
		TimeEventManager.OnDayEnd += RevertTiles;
		GridEventManager.OnWorldObjectRemove += RemoveObjectInCell;
	}

	// Use this for initialization
	private void GetWorldTiles () 
	{
		GetTilemapTiles(instance.groundTilemap, instance.groundTiles);
		GetTilemapTiles(instance.wallTilemap, instance.wallTiles);		
		GetTilemapTiles(instance.objectTilemap, instance.objectTiles);
	}

	private void UpdateWorldTiles ()
	{
		UpdateTilemapTiles(instance.groundTilemap, instance.groundTiles);
		UpdateTilemapTiles(instance.wallTilemap, instance.wallTiles);
		UpdateTilemapTiles(instance.objectTilemap, instance.objectTiles);
	}

	private void GetTilemapTiles(Tilemap tileMap, Dictionary<Vector3, WorldTile> tileDict)
	{
		tileMap.CompressBounds();
		Tilemap groundTile = instance.groundTilemap;
		foreach (Vector3Int pos in groundTile.cellBounds.allPositionsWithin)
		{
			var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

			/*if (!tileMap.HasTile(localPlace){
				continue;
			}*/
			var tile = new WorldTile()
			{
				LocalPlace = localPlace,
				WorldLocation = Vector3Int.RoundToInt(groundTile.CellToWorld(localPlace)),
				TileBase = tileMap.GetTile(localPlace),
				TilemapMember = tileMap,
				Name = localPlace.x + "," + localPlace.y,
				WorldObject = tileMap == instance.objectTilemap ? GetObjectInCell(Vector3Int.RoundToInt(groundTile.CellToWorld(localPlace))) : null,
				DefaultWorldObjectData = (tileMap == instance.objectTilemap && GetObjectInCell(Vector3Int.RoundToInt(groundTile.CellToWorld(localPlace))) != null) ? GetObjectInCell(Vector3Int.RoundToInt(groundTile.CellToWorld(localPlace))).Data : null,
				WaterLevel = 0
			};
			if(tileDict.TryGetValue(tile.WorldLocation, out WorldTile tryTile))
				tryTile = tile;
			else
				tileDict.Add(tile.WorldLocation, tile);
		}
	}

	private void UpdateTilemapTiles(Tilemap tileMap, Dictionary<Vector3, WorldTile> tileDict)
	{
		tileMap.CompressBounds();
		Tilemap groundTile = instance.groundTilemap;
		foreach(Vector3Int pos in groundTile.cellBounds.allPositionsWithin)
		{
			var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
			var worldLoc = Vector3Int.RoundToInt(groundTile.CellToWorld(localPlace));
			if(tileDict.TryGetValue(worldLoc, out WorldTile tryTile)) // ensure tile exists in tilemap
			{
				tileMap.SetTile(localPlace, tryTile.TileBase);
				tryTile.TilemapMember = tileMap;
				// do some object-specific stuff, maybe should be different method
				if(tileMap == instance.objectTilemap)
				{
					WorldObject thisObj = GetObjectInCell(worldLoc);
					if(thisObj != null)
					{
						if(tryTile.DefaultWorldObjectData == null)
						{						
							Destroy(GetObjectInCell(worldLoc).gameObject);
							RemoveObjectInCell(worldLoc);
						}
						else if(thisObj.Data != tryTile.DefaultWorldObjectData)
						{
							// call method on WorldObject to make it change to reflect tryTile.DefaultWorldObjectData
							ReflectObjectData(thisObj, tryTile);
						}
					}
					else if(tryTile.DefaultWorldObjectData != null)
					{
						// call method to create a WorldObject at this tile and have it reflect tryTile.DefaultWorldObjectData
						GameObject gameObj = Instantiate(emptyWorldObjectPrefab, tryTile.WorldLocation + TileOffset, Quaternion.identity);
						if(tryTile.DefaultWorldObjectData is PlantScriptableObject)
						{
							Destroy(gameObj.GetComponent<WorldObject>());
							thisObj = gameObj.AddComponent<PlantObject>();
						}
						else
							thisObj = gameObject.GetComponent<WorldObject>();
                        thisObj.transform.SetParent(GridManager.instance.objectTilemap.gameObject.transform);
						ReflectObjectData(thisObj, tryTile);
					}	
				}
			}
		}
	}

	private void ReflectObjectData(WorldObject thisObj, WorldTile tryTile)
	{
		thisObj.name = tryTile.DefaultWorldObjectData.name;
		thisObj.TileData(tryTile);
        tryTile.WorldObject = thisObj;
	}

	private void RemoveObjectInCell(Vector3Int position)
	{
		if(instance.objectTiles.TryGetValue(position, out WorldTile wTile))
		{
			if(wTile.DefaultWorldObjectData != null)
			{
				wTile.DefaultWorldObjectData = null;
				wTile.WorldObject = null;
				wTile.Parameters.Clear();
			}
		}
	}

	private void RemoveObjectInCell(Object sender, Vector3Int position)
	{
		if(sender.GetType() == typeof(WorldObject) || sender.GetType().IsSubclassOf(typeof(WorldObject)))
		{
			RemoveObjectInCell(position);
		}		
	}
	private WorldObject GetObjectInCell(Vector3Int position)
    {
		this.position = position;
        int childCount = instance.objectTilemap.transform.childCount;
		//Debug.Log("World - Min: " + min + ", Max: " + max);
        Bounds bounds = new Bounds(position + (new Vector3(1, 0, 1) * 0.5f), Vector3.one);

        for (int i = 0; i < childCount; i++)
        {
            Transform child = instance.objectTilemap.transform.GetChild(i);
            if (bounds.Contains(child.position))
			{
                return child.GetComponent<WorldObject>();

			}
        }
        return null;
    }
}


