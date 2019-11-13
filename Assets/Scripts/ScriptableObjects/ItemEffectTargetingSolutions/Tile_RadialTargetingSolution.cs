using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tile_RadialTargetingSolution : Tile_TargetingSolution
{
    public override List<WorldTile> GetTargets(Vector3 startLocation)
    {
        List<WorldTile> targets = new List<WorldTile>();
        Vector3Int startTileLoc = Vector3Int.FloorToInt(startLocation);
        startTileLoc = new Vector3Int(startTileLoc.x, 0, startTileLoc.z);

        var tiles = GridManager.instance.tiles;

        // based on effect, this may target the base tiles, the tile's WorldObject, enemies, etc!      
        Vector3Int tileLoc;
        for(int i = -(int)size; i < (int)size; ++i)
        {
            for(int j = -(int)size; j < (int)size; ++j)
            {
                if(Mathf.Abs(i) + Mathf.Abs(j) < size)
                {
                    tileLoc = new Vector3Int(startTileLoc.x + i, 0, startTileLoc.z + j);
                    if (tiles.TryGetValue(tileLoc, out WorldTile _tile))
                    {
                        targets.Add(_tile);
                    }                    
                }
            }
        } 

        return targets;       
    }

    public override List<WorldTile> GetTargets(Vector3 startLocation, LayerMask whatToHit)
    {
        List<WorldTile> retList = GetTargets(startLocation);
        foreach(WorldTile w in GetTargets(startLocation))
        {
            if(w.WorldObject == null)
                retList.Remove(w);
            else
            {
                if (whatToHit != (whatToHit | (1 << w.WorldObject.gameObject.layer)))
                {
                    retList.Remove(w);
                }
            }
        }
        return retList;
    }
}
