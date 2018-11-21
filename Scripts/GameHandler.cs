using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    [SerializeField] private Transform goldNodeTransform;
    [SerializeField] private Transform storageTransform;
    
    public static Transform GetResourceNode_Static()
    {
        return instance.GetResourceNode();
    }

    public static Transform GetStorageNode_Static()
    {
        return instance.GetStorageNode();
    }

    private void Awake()
    {
        instance = this;
    }

    private Transform GetResourceNode()
    {
        return goldNodeTransform;
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
