using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldObjectScriptableObject : ScriptableObject
{    
    [SerializeField] public Sprite sprite;
    [SerializeField] public Collider collider;
    [SerializeField] public bool hasPostDeath;
    [SerializeField] public TileEffectTargetStruct[] targetability;
    [SerializeField] public int health;
    [SerializeField] public Item[] itemProducts;

    [SerializeField] public Sprite[] damageFrameArray;
    [SerializeField] public Sprite[] deathFrameArray;
    [SerializeField] public Vector3[] shadowParams;
    [SerializeField] public int shadowTrigger;
    [SerializeField] public Sprite postDeathSprite;
    [SerializeField] public Sprite[] postDeathFrameArray;
    [SerializeField] public float frameRate;
    [SerializeField] public Color outlineColor;

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
