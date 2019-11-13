using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class PlantScriptableObject : ScriptableObject
{
    public new string name;
    public int maxLife;
    public PlantTiers[] tiers;

    public TileBase wateredSoil;

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/WorldObjects/Plants")]
    public static void CreatePlant()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Plant Scriptable Object", "New Plant Scriptable Object", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PlantScriptableObject>(), path);
    }
    #endif
}
