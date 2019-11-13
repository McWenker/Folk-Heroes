using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class WorldObjectCreationEffect : Effect
{
    [SerializeField] private WorldObject objectToCreate;
    [SerializeField] private Tile_TargetingSolution[] targetSolutions;
    [SerializeField] private TileBase[] legalTiles;
    public override void DoEffect(Vector3 effectLoc, LayerMask whoToHit, float chargeMult)
    {
        for(int i = 0; i < targetSolutions.Length; ++i)
        {
            foreach(WorldTile w in targetSolutions[i].GetTargets(effectLoc))
            {
                if(legalTiles.Contains(w.TileBase) || legalTiles.Length == 0)
                {
                    if(w.WorldObject == null)
                    {
                        w.WorldObject = Instantiate(objectToCreate.gameObject, w.WorldLocation + (GridManager.TileOffset), Quaternion.identity).GetComponent<WorldObject>();
                        w.WorldObject.gameObject.transform.SetParent(GridManager.instance.objectTilemap.gameObject.transform);
                        w.WorldObject.TileData(w);
                    }
                }                
            }
        }
    }

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/ItemEffects/WorldObjectCreationEffect")]
    public static void CreateTileEffect()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save World Object Creation Effect", "New World Object Creation Effect", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WorldObjectCreationEffect>(), path);
    }
    #endif
}
