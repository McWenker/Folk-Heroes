using System;
using UnityEngine;

public interface IAttack
{
    void AttackCompleted();
    void DashAttack(Vector3 startPos, Vector3 target, float maxDistance, Action animation);
    float DashDistance();
    void DistanceCheck();
    void PlayAnimationAttack(Vector3 lookAtPosition, Action onAnimationCompleted);
    void SearchForFoes();
    Transform GetTarget();
    bool AttackReady();
}
