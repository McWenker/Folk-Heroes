using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayState : GameState
{
    PlayerCharacter player;
    public override void Enter()
	{
		base.Enter ();
		StartCoroutine(PlayGame());
	}

    IEnumerator PlayGame()
    {
        player = FindObjectOfType<PlayerCharacter>();
        yield return null;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        grid = owner.gameObject.GetComponent<GridManager>();
        grid.InitSceneGrid(scene.name);
        foreach(SceneDestination sd in FindObjectsOfType<SceneDestination>())
        {
            if(owner.DestinationName == sd.Destination)
            {
                player.transform.position = sd.transform.position;
                break;
            }
        }
    }

    void PauseInput(Object sender, int sceneIndex, string dest)
    {
        owner.Input.enabled = false;
    }

    void SceneTransitionFinish(Object sender)
    {
        owner.Input.enabled = true;
    }

    protected override void AddListeners()
    {
        GameplayEventManager.OnSceneChange += PauseInput;
        GameplayEventManager.OnSceneTransitionFinish += SceneTransitionFinish;
        SceneManager.sceneLoaded += SceneLoaded;
    }

    protected override void RemoveListeners()
    {
        GameplayEventManager.OnSceneChange -= PauseInput;
        GameplayEventManager.OnSceneTransitionFinish -= SceneTransitionFinish;
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}