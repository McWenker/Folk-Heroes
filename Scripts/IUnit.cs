using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    void Idling();
    bool IsIdle();
    void MiningCompleted();
    void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition);
    void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted);
}
