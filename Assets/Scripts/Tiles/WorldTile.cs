using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WorldTile {
    public Vector3Int LocalPlace { get; set; }

    public Vector3Int WorldLocation { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }

    public string Name { get; set; }

    public WorldObjectScriptableObject DefaultWorldObjectData { get; set; }

    public WorldObjectData DynamicWorldObjectData { get; set; }

    public WorldObject WorldObject { get; set; }

    public int WaterLevel { get; set; }

    public WorldTile()
    {
        DynamicWorldObjectData = new WorldObjectData(DefaultWorldObjectData);
        TimeEventManager.OnDayEnd += DayEnd;
    }   

    public void Water(int waterAmt)
    {
        WaterLevel += waterAmt;
    }

    private void DayEnd(Object sender)
    {
        if(DefaultWorldObjectData != null && DynamicWorldObjectData != null)
            DynamicWorldObjectData.OnDayEnd(WaterLevel);
        
        WaterLevel = Mathf.Clamp(--WaterLevel, -5, 5);
    }

}
