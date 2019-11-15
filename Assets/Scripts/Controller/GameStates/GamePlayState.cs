using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayState : GameState
{
    public override void Enter()
	{
		base.Enter ();
		StartCoroutine(PlayGame());
	}

    IEnumerator PlayGame()
    {
        yield return null;
    }

    protected override void AddListeners()
    {
        //GameplayEventManager.OnNPCSpeak += NPCSpeak;
    }

    protected override void RemoveListeners()
    {
        //GameplayEventManager.OnNPCSpeak -= NPCSpeak;
    }
}