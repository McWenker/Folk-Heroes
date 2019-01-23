using System;
using UnityEngine;

public interface IUnit
{
    void ClearMove();
    void Idling();
    void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition);

    void OnDeath();
    bool IsIdle { get; }
}
