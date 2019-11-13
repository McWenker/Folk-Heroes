using UnityEngine;
using UnityEditor;

public class ProjectileScriptableObject : ScriptableObject
{
    public float speed;
    public AnimationCurve chargeGraph;
    public float chargeMultiplier;
    public float sway;
    public float maxFlightDuration;
    public Sprite[] defaultSprites;
    public Sprite[] dudSprites;
    public Effect[] effects;
    public LayerMask whoToIgnore;

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/Items/Projectile")]
    public static void CreateProjectile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Projectile Scriptable Object", "New Projectile Scriptable Object", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ProjectileScriptableObject>(), path);
    }
    #endif
}
