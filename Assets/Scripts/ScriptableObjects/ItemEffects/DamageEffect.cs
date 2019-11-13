using UnityEngine;
using UnityEditor;

[System.Serializable]
public class DamageEffect : Effect
{ 
    [SerializeField] private int baseDamage;
    [SerializeField] private float baseDazeTime;
    [SerializeField] private Entity_TargetingSolution[] targetSolutions;

    public override void DoEffect(Vector3 effectLocation, LayerMask whatIsTarget, float chargePercent)
    {
        Debug.Log(chargePercent + ", " + (int)(chargeGraph.Evaluate(chargePercent)*chargeMultiplier));
        int damage = baseDamage;
        damage += (int)(chargeGraph.Evaluate(chargePercent)*chargeMultiplier);
        
        for(int i = 0; i < targetSolutions.Length; ++i)
        {
            foreach(Transform t in targetSolutions[i].GetTargets(effectLocation, whatIsTarget))
            {
                //damage enemies!
                if(t.GetComponent<Health>() != null && t.GetComponent<WorldObject>() == null)
                {
                    t.GetComponent<Health>().Damage(damage);
                }
            }
        }        
    }

    #if UNITY_EDITOR   
    [MenuItem("Assets/Create/ItemEffects/DamageEffect")]
    public static void CreateDamageEffect()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Damage Effect", "New Damage Effect", "asset", "Please enter name: ");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DamageEffect>(), path);
    }
    #endif
}
