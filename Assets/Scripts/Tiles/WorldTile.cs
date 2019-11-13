using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile {
    public Vector3Int LocalPlace { get; set; }

    public Vector3Int WorldLocation { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }

    public string Name { get; set; }

    public WorldObject WorldObject { get; set; }
}
