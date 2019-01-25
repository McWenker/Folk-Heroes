using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResource
{
    GameResourceType resourceType;

    public GameResourceType ResourceType
    {
        get { return resourceType; }
    }

    public GameResource(GameResourceType type)
    {
        resourceType = type;
    }
}