﻿using System;
using UnityEngine;

public interface IGather
{
    void MiningCompleted();
    void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted);
}
