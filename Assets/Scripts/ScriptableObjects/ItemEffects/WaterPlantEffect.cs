using UnityEngine;
using UnityEditor;

public class WaterPlantEffect : Effect
{
    [SerializeField] private Tile_TargetingSolution[] targetSolutions;
    [SerializeField] int waterAmount;
    public override void DoEffect(Vector3 effectLocation, LayerMask whichPlants, float chargeMult)
    {
        for(int i = 0; i < targetSolutions.Length; ++i)
        {
            foreach(WorldTile w in targetSolutions[i].GetTargets(effectLocation))
            {
                if(w.WorldObject != null)
                {
                    if(w.WorldObject.GetComponent<PlantObject>() != null)
                        w.WorldObject.GetComponent<PlantObject>().Water(waterAmount);
                }                
            }
        }
    }

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/ItemEffects/WaterPlantEffect")]
    public static void CreateWaterPlantEffect()
    {
        string path = EditorUtility.SaveFilePanelInProject("Water Plant Change Effect", "Water Plant Change Effect", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterPlantEffect>(), path);
    }
    #endif
}
