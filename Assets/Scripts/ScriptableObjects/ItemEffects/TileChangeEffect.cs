using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;

public class TileChangeEffect : Effect
{
    public TilemapLayers tilemapLayer;
    public TileBase[] tilesToChange;
    public TileBase[] changedTiles;
    [SerializeField] private Tile_TargetingSolution[] targetSolutions;
    [SerializeField] WorldObject[] worldObjectsToIgnore;

    public override void DoEffect(Vector3 effectLocation, LayerMask whatIsTarget, float chargeMult)
    {               
        // based on effect, this may target the base tiles, the object tiles, enemies, etc!        
        for(int i = 0; i < targetSolutions.Length; ++i)
        {
            foreach(WorldTile w in targetSolutions[i].GetTargets(effectLocation))
            {
                if(w.WorldObject == null || whatIsTarget == (whatIsTarget| (1 << w.WorldObject.gameObject.layer)))
                {
                    for(int j = 0; j < tilesToChange.Length; ++j)
                    {
                        if(w.TileBase == tilesToChange[j])
                        {
                            w.TilemapMember.SetTileFlags(w.LocalPlace, TileFlags.None);
                            w.TilemapMember.SetTile(w.LocalPlace, changedTiles[i]);
                            w.TileBase = changedTiles[i];
                            w.TilemapMember.SetTileFlags(w.LocalPlace, TileFlags.LockAll);
                        }
                    }
                }                
            }
        }
        
    }

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/ItemEffects/TileChangeEffect")]
    public static void CreateTileChangeEffect()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile Change Effect", "New Tile Change Effect", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileChangeEffect>(), path);
    }
    #endif
}
