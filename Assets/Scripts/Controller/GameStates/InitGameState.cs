using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGameState : GameState
{
    public override void Enter()
	{
		base.Enter ();
		StartCoroutine(GameInit());
	}

    IEnumerator GameInit()
    {
        SceneManager.LoadScene(1);
        owner.Input.enabled = true;
        yield return null;
        owner.ChangeState<GamePlayState>();
    }
}