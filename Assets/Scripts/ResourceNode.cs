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
    private GameResourceType resourceType;
    private int resourceAmount;
    public GameResourceType ResourceType
    {
        get { return resourceType; }
    }

    public ResourceNode(Transform resourceNodeTransform, GameResourceType resourceType, int resourceAmount)
    {
        this.resourceNodeTransform = resourceNodeTransform;
        this.resourceType = resourceType;
        this.resourceAmount = resourceAmount;
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
        {
            switch(resourceType)
            {
                case GameResourceType.Gold:
                    resourceNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.goldNodeDepletedSprite;
                    break;
                case GameResourceType.Iron:
                    resourceNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.ironNodeDepletedSprite;
                    break;
                case GameResourceType.Mana:
                    resourceNodeTransform.GetComponent<SpriteRenderer>().sprite = GameAssets.i.manaNodeDepletedSprite;
                    break;
                default:
                    resourceNodeTransform.GetComponent<SpriteRenderer>().enabled = false;
                    break;
            }
        }
        return true;
    }

    public bool HasResources()
    {
        return resourceAmount > 0;
    }
}
