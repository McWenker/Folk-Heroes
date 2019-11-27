using UnityEngine;

public class PlantObject : WorldObject
{
    PlantTiers currentTier;
    PlantScriptableObject castData;
    PlantObjectData dynamicData;

    public override void TileData(WorldTile thisTile)
    {
        base.TileData(thisTile);

        castData = (PlantScriptableObject)data;
        dynamicData = thisTile.DynamicWorldObjectData as PlantObjectData;
        currentTier = dynamicData.CurrentTier;
        animator.SetSprite(currentTier.tierSprite);
    }       

    private void UpdatePlant(Object sender)
    {
        if(gameObject != null && this != null)
        {
            currentTier = dynamicData.CurrentTier;
            animator.SetSprite(currentTier.tierSprite);
        }
    }
}
