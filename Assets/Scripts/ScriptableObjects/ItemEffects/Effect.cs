using UnityEngine;

public abstract class Effect : ScriptableObject, I_Effect
{
    [SerializeField] protected AnimationCurve chargeGraph;
    [SerializeField] protected int chargeMultiplier;
    public abstract void DoEffect(Vector3 effectLocation, LayerMask whatIsTarget, float chargePercent);
}
