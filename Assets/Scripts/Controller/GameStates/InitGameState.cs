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
        grid = owner.gameObject.AddComponent<GridManager>();
        day = owner.gameObject.AddComponent<DayController>();
        
        yield return null;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        grid.InitSceneGrid(scene.name);
        Instantiate(owner.GameplayUIPrefab, Vector3.zero, Quaternion.identity);
        Transform playController = Instantiate(owner.PlayControllerPrefab, Vector3.zero, Quaternion.identity).transform;
        playController.parent = owner.transform;
        Instantiate(owner.PlayerPrefab, Vector3.zero, Quaternion.identity);
        InputEventManager.ActionBarPress(this, 1);
        owner.Input.enabled = true;
        owner.ChangeState<GamePlayState>();
    }

    protected override void AddListeners()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    protected override void RemoveListeners()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}