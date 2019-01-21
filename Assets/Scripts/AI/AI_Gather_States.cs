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
        Fleeing,
    }

    private IUnit unit;
    private IGather gather;
    private IVision vision;
    [SerializeField] private State state;
    private State prevState;
    private ResourceNode resourceNode;
    private Transform storageNodeTransform;
    private int inventoryAmount = 0;
    private TextMeshPro inventoryTextMesh;
    [SerializeField] private Transform hostile;
    [SerializeField] private SpriteRenderer fleeSprite;
    private bool fleeingCooldown = false;

    // Use this for initialization
    private void Awake ()
    {
        unit = GetComponent<IUnit>();
        gather = GetComponent<IGather>();
        vision = GetComponent<IVision>();
        state = State.Idle;
        inventoryTextMesh = GetComponentInChildren<TextMeshPro>();
	}

    private void UpdateInventoryText()
    {
        if (inventoryAmount > 0)
            inventoryTextMesh.SetText(inventoryAmount.ToString());
        else
            inventoryTextMesh.SetText("");
    }

    private void Update()
    {
        DetermineInventory();
        hostile = vision.SearchForFoes();
        DetermineFlee();
        StateSwitch();
    }

    private void DetermineInventory()
    {
        if(inventoryAmount >= 3)
        {
            FullInventory();
        }
    }

    private void FullInventory()
    {
        storageNodeTransform = GameHandler.GetStorageNode_Static();
        resourceNode = GameHandler.GetResourceNodeNearPosition_Static(resourceNode.GetPosition());
        state = State.MovingToStorage;
        unit.Idling();
    }

    private void DetermineFlee()
    {
        if(hostile != null && state != State.Fleeing)
        {
            state = State.Fleeing;
        }

        if(state == State.Fleeing)
        {
            fleeSprite.enabled = true;
        }
        else
            fleeSprite.enabled = false;
    }

    private void StateSwitch()
    {
        switch (state)
        {
            case State.Idle:
                unit.Idling();
                resourceNode = GameHandler.GetResourceNodeNearPosition_Static(transform.position);
                if(resourceNode != null)
                    state = State.MovingToResourceNode;
                break;
            case State.MovingToResourceNode:
                unit.MoveTo(resourceNode.GetPosition(), 1.5f, () =>
                {
                    state = State.GatheringResources;
                });
                break;
            case State.GatheringResources:
                if(inventoryAmount >= 3)
                {
                    FullInventory();
                    break;
                }
                else
                {
                    if (resourceNode != null && resourceNode.HasResources())
                    {
                        gather.PlayAnimationMine(resourceNode.GetPosition(), () =>
                        {
                            if (resourceNode.GrabResource())
                            {
                                gather.AddToInventory(new GameResource(resourceNode.ResourceType));
                                inventoryAmount++;
                                UpdateInventoryText();
                            }
                            else
                                state = State.Idle;
                        });
                    }
                    else
                        state = State.Idle;
                }              
                break;
            case State.MovingToStorage:
                unit.MoveTo(storageNodeTransform.position, 2f, () =>
                {
                    gather.UnloadInventory();
                    inventoryAmount = 0;
                    UpdateInventoryText();
                    state = State.Idle;
                });
                break;
            case State.Fleeing:
                if(hostile == null)
                {
                    state = State.Idle;
                }
                else
                {
                    if(!fleeingCooldown)
                    {
                        Vector3 dirToFoe = (transform.position - hostile.position);
                        dirToFoe *= 2f;
                        Vector3 posToRun = (transform.position + dirToFoe);
                        StartCoroutine(FleeingCooldown());
                        unit.MoveTo(posToRun, 1f, () => 
                        {
                            hostile = vision.SearchForFoes();
                            if(hostile == null)
                            {
                                state = State.Idle;
                            }
                            
                        });
                    }                    
                }
                break;
        }
    }
    private IEnumerator FleeingCooldown()
    {
        fleeingCooldown = true;
        yield return new WaitForSeconds(0.2f);
        fleeingCooldown = false;
    }

    public void SetResourceNode(ResourceNode resourceToGet)
    {
        resourceNode = resourceToGet;
    }
}
