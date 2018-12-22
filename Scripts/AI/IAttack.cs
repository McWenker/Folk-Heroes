using System;
using UnityEngine;

public interface IAttack
{
    void AttackCompleted();
    void DashAttack(Vector3 startPos, Vector3 target, float maxDistance, Action animation);
    float DashDistance();
    bool DistanceCheck();
    void PlayAnimationAttack(Vector3 lookAtPosition, Action onAnimationCompleted);
    Transform SearchForFoes();
    bool AttackReady();
}
