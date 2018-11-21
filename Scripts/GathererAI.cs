using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererAI : MonoBehaviour
{
    private enum State
    {
        Idle,
        MovingToResourceNode,
        GatheringResources,
        MovingToStorage,
    }

    private IUnit unit;
    private State state;
    private Transform resourceNodeTransform;
    private Transform storageNodeTransform;
    private int goldInventoryAmount;

    // Use this for initialization
    private void Awake ()
    {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;
	}

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                unit.Idling();
                resourceNodeTransform = GameHandler.GetResourceNode_Static();
                state = State.MovingToResourceNode;
                break;
            case State.MovingToResourceNode:
                if (unit.IsIdle())
                {
                    unit.MoveTo(resourceNodeTransform.position, 1f, () =>
                    {
                        state = State.GatheringResources;
                    });
                }
                break;
            case State.GatheringResources:
                if (unit.IsIdle())
                {
                    if(goldInventoryAmount > 0)
                    {
                        // move to storage
                        storageNodeTransform = GameHandler.GetStorageNode_Static();
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        unit.PlayAnimationMine(resourceNodeTransform.position, () =>
                        {
                            goldInventoryAmount++;
                        });
                    }                    
                }
                break;
            case State.MovingToStorage:
                if (unit.IsIdle())
                {
                    unit.MoveTo(storageNodeTransform.position, 0.3f, () =>
                    {
                        goldInventoryAmount = 0;
                        state = State.Idle;
                    });
                }
                break;

        }
    }
}
