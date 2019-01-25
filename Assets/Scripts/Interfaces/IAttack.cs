using System;
using UnityEngine;

public interface IAttack
{
    void AttackCompleted();
    void Attack(Action onAttackCompleted);
    void CommenceAttack(Vector3 target, Action animation);
    void PlayAnimationAttack(Vector3 lookAtPosition, Action onAnimationCompleted);
    float AttackRange { get; set; }
    bool AttackReady { get; set; }
    bool Attacking { get; set; }
}
