using UnityEngine;
 #if UNITY_EDITOR
 using UnityEditor;
 #endif

[CreateAssetMenu]
public class ItemScriptableObject : ScriptableObject
{
    private const int minUses = 1;
    private const int maxUses = 2;
    public string itemName;
    public ItemSprites itemSprites;
    public UseTypes[] useTypes; // left click or right click, basically
    public float maxRange;
    public float resetTime;
}

[System.Serializable]
public class UseTypes
{
    public string name;
    public ItemUses[] uses;
}

[System.Serializable]
public class ItemSprites
{
    public Sprite inventorySprite;
    public Sprite leftHandSprite;
    public Sprite rightHandSprite;
    public bool flipSprites;
    public bool wrapSwing;
}

#if UNITY_EDITOR
 [CustomEditor(typeof(ItemScriptableObject))]
 public class RandomScript_Editor : Editor
 {
     public override void OnInspectorGUI()
     {
         DrawDefaultInspector(); // for other non-HideInInspector fields
 
         ItemScriptableObject script = (ItemScriptableObject)target;
 
         // draw checkbox for the bool
         /*for(int i = 0; i < script.useTypes.Length; ++i)
         {
            for(int j = 0; j < script.useTypes[i].uses.Length; ++j)
            {
                script.useTypes[i].uses[j].canBeHeld = EditorGUILayout.Toggle("Can Be Held", script.useTypes[i].uses[j].canBeHeld);
                if(script.useTypes[i].uses[j].canBeHeld)
                {

                }
            }
         }*/
     }
 }
 #endif
