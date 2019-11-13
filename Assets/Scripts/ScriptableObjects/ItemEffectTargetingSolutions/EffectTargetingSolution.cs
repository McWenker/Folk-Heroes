using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EffectTargetingSolution<T> : ScriptableObject
{
    [SerializeField] protected float size;
    public abstract List<T> GetTargets(Vector3 startLocation);

    public abstract List<T> GetTargets(Vector3 startLocation, LayerMask whatToHit);
}
