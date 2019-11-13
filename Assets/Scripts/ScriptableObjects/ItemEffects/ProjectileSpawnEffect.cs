using UnityEngine;
using UnityEditor;

public class ProjectileSpawnEffect : Effect
{
    public Projectile[] projectiles;

    public override void DoEffect(Vector3 effectLocation, LayerMask whatIsTarget, float chargeMult)
    {
        // used for other effects that happen when a projectile is spawned, graphics, etc
    }

    public void SpawnProjectile(Vector3 spawnLocation, Vector3 targetPoint, LayerMask whatIsTarget, float chargeMult)
    {
        for(int i = 0; i < projectiles.Length; ++i)
        {
            Instantiate(projectiles[i].gameObject, spawnLocation, Quaternion.identity).GetComponent<Projectile>().Fly(whatIsTarget, targetPoint, chargeMult);
        }
    }

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/ItemEffects/ProjectileSpawnEffect")]
    public static void CreateProjectileSpawnEffect()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Projectile Spawn Effect", "New Projectile Spawn Effect", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ProjectileSpawnEffect>(), path);
    }
    #endif
}
