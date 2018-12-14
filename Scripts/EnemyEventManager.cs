using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyEventManager
{
    public delegate void AggroActionEventHandler(GameObject sender, Transform target);
    public static event AggroActionEventHandler OnAggro;

    public static void NewAggro(GameObject sender, Transform targetTransform)
    {
        if (OnAggro != null) OnAggro(sender, targetTransform);
    }
}
