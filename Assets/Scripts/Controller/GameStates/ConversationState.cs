using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConversationState : GameState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(InitConvo());
    }

    IEnumerator InitConvo()
    {   
        owner.Input.enabled = false;
        yield return null;

    }
}