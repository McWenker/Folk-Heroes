using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FolkScriptableObject : ScriptableObject
{
    // animation information, spritesheets etc
    public Sprite convoSprite; 
    //[SerializeField] Conversation[] conversations;
    public string description;
    // home location
    // arrays of liked, disliked items
    public Item[] likedItems;
    public Item[] dislikedItems;
    public Schedule[] schedules;
    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/Folk")]
    public static void CreateFolk()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Folk Scriptable Object", "New Folk Scriptable Object", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FolkScriptableObject>(), path);
    }
    #endif
}

