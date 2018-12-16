using System;
using UnityEngine;

public interface IUnit
{
    void Idling();
    bool IsIdle();
    void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition);
}
