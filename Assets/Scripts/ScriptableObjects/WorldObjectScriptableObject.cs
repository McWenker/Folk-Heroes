using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldObjectScriptableObject : ScriptableObject
{    
    [SerializeField] public bool hasPostDeath;
    [SerializeField] public TileEffectTargetStruct[] targetability;
    [SerializeField] public Item[] itemProducts;

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/WorldObjects/GenericObjects")]
    public static void CreateWorldObject()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save World Object Scriptable Object", "New World Object Scriptable Object", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WorldObjectScriptableObject>(), path);
    }
    #endif
}
