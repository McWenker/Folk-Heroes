using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class ResourceNode
{
    public static event EventHandler OnResourceNodeClicked;
    private Transform resourceNodeTransform;
    private int resourceAmount;

    public ResourceNode(Transform resourceNodeTransform)
    {
        this.resourceNodeTransform = resourceNodeTransform;
        resourceAmount = 6;
        resourceNodeTransform.GetComponent<Button_Sprite>().ClickFunc = () =>
        {
            if (OnResourceNodeClicked != null) OnResourceNodeClicked(this, EventArgs.Empty);
        };
    }

    public Vector3 GetPosition()
    {
        return resourceNodeTransform.position;
    }

    public bool GrabResource()
    {
        if (resourceAmount > 0)
        {
            resourceAmount--;
        }
        else
            return false;
        if (resourceAmount <= 0)
            resourceNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.goldNodeDepletedSprite;
        return true;
    }

    public bool HasResources()
    {
        return resourceAmount > 0;
    }
}
