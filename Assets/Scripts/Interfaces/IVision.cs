using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVision
{
    bool DistanceCheck();
    bool DistanceCheck(Vector3 position, float range);
	Transform SearchForFoes();
}
