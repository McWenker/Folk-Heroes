using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AI_Gather_States : MonoBehaviour
{
    private enum State
    {
        Idle,
        MovingToResourceNode,
        GatheringResources,
        MovingToStorage,
    }

    private IUnit unit;
    private IGather gather;
    private State state;
    private ResourceNode resourceNode;
    private Transform storageNodeTransform;
    private int goldInventoryAmount;
    private TextMeshPro inventoryTextMesh;

    // Use this for initialization
    private void Awake ()
    {
        unit = GetComponent<IUnit>();
        gather = GetComponent<IGather>();
        state = State.Idle;
        inventoryTextMesh = GetComponentInChildren<TextMeshPro>();
	}

    private void UpdateInventoryText()
    {
        if (goldInventoryAmount > 0)
            inventoryTextMesh.SetText(goldInventoryAmount.ToString());
        else
            inventoryTextMesh.SetText("");
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                //resourceNode = GameHandler.GetResourceNode_Static();
                if(resourceNode != null)
                    state = State.MovingToResourceNode;
                break;
            case State.MovingToResourceNode:
                if (unit.IsIdle())
                {
                    unit.MoveTo(resourceNode.GetPosition(), 1.2f, () =>
                    {
                        state = State.GatheringResources;
                    });
                }
                break;
            case State.GatheringResources:
                if (unit.IsIdle())
                {
                    if(goldInventoryAmount >= 3)
                    {
                        // move to storage
                        storageNodeTransform = GameHandler.GetStorageNode_Static();
                        resourceNode = GameHandler.GetResourceNodeNearPosition_Static(resourceNode.GetPosition());
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        gather.PlayAnimationMine(resourceNode.GetPosition(), () =>
                        {
                            if (resourceNode != null)
                            {
                                if (resourceNode.GrabResource())
                                {
                                    goldInventoryAmount++;
                                    UpdateInventoryText();
                                }
                                else
                                    state = State.Idle;
                            }                                
                        });
                    }                    
                }
                break;
            case State.MovingToStorage:
                if (unit.IsIdle())
                {
                    unit.MoveTo(storageNodeTransform.position, 0.3f, () =>
                    {
                        GameResourceBank.AddAmount(GameResource.Gold, goldInventoryAmount);
                        goldInventoryAmount = 0;
                        UpdateInventoryText();
                        state = State.Idle;
                    });
                }
                break;

        }
    }

    public void SetResourceNode(ResourceNode resourceToGet)
    {
        resourceNode = resourceToGet;
    }
}
