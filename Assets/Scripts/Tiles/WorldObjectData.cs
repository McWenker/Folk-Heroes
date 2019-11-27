using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectData
{   
    protected WorldObjectScriptableObject defaultData;
    public bool isDead;
    public int health;

    public virtual void OnDayEnd(int waterLevel)
    {

    }

    public WorldObjectData(WorldObjectScriptableObject defaultData)
    {
        this.defaultData = defaultData;
    }
}
