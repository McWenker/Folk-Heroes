using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    [SerializeField] private Transform[] goldNodeTransformArray;
    [SerializeField] private Transform[] ironNodeTransformArray;
    [SerializeField] private Transform[] manaNodeTransformArray;
    [SerializeField] private Transform storageTransform;
    [SerializeField] private int minRandom;
    [SerializeField] private int maxRandom;

    private List<ResourceNode> resourceNodeList;

    public static ResourceNode GetResourceNode_Static()
    {
        return instance.GetResourceNode();
    }

    public static ResourceNode GetResourceNodeNearPosition_Static(Vector3 position)
    {
        return instance.GetResourceNodeNearPosition(position);
    }

    public static Transform GetStorageNode_Static()
    {
        return instance.GetStorageNode();
    }

    private void Awake()
    {
        instance = this;

        resourceNodeList = new List<ResourceNode>();
        foreach(Transform goldNodeTransform in goldNodeTransformArray)
        {
            resourceNodeList.Add(new ResourceNode(goldNodeTransform, GameResourceType.Gold, UnityEngine.Random.Range(minRandom, maxRandom)));
        }
        foreach (Transform ironNodeTransform in ironNodeTransformArray)
        {
            resourceNodeList.Add(new ResourceNode(ironNodeTransform, GameResourceType.Iron, UnityEngine.Random.Range(minRandom, maxRandom)));
        }
        foreach (Transform manaNodeTransform in manaNodeTransformArray)
        {
            resourceNodeList.Add(new ResourceNode(manaNodeTransform, GameResourceType.Mana, UnityEngine.Random.Range(minRandom, maxRandom)));
        }

        ResourceNode.OnResourceNodeClicked += ResourceNode_OnResourceNodeClicked;
    }

    private void ResourceNode_OnResourceNodeClicked(object sender, EventArgs e)
    {
        ResourceNode resourceNode = sender as ResourceNode;
    }

    private ResourceNode GetResourceNode()
    {
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);
        for (int i = 0; i < tmpResourceNodeList.Count; i++)
        {
            if (!tmpResourceNodeList[i].HasResources())
            {
                tmpResourceNodeList.RemoveAt(i);
                i--;
            }
        }

        if (tmpResourceNodeList.Count > 0)
        {
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];
        }
        else
            return null;
    }

    private ResourceNode GetResourceNodeNearPosition(Vector3 position)
    {
        float maxDistance = 20f;
        List<ResourceNode> tmpResourceNodeList = new List<ResourceNode>(resourceNodeList);
        for(int i = 0; i < tmpResourceNodeList.Count; i++)
        {
            if(!tmpResourceNodeList[i].HasResources() || Vector3.Distance(position, tmpResourceNodeList[i].GetPosition()) > maxDistance)
            {
                tmpResourceNodeList.RemoveAt(i);
                i--;
            }
        }

        if (tmpResourceNodeList.Count > 0)
        {
            return tmpResourceNodeList[UnityEngine.Random.Range(0, tmpResourceNodeList.Count)];
        }
        else
            return null;
    }

    private Transform GetStorageNode()
    {
        return storageTransform;
    }

    private void SpriteAnimator_OnAnimationLooped(object sender, System.EventArgs e)
    {
        Debug.Log("OnAnimationLooped");
    }
}
