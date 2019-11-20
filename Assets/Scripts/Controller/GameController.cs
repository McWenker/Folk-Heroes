using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : StateMachine
{
    InputController input;
    [SerializeField] string lastDestinationName;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playControllerPrefab;
    [SerializeField] GameObject gameplayUIPrefab;
    public InputController Input
    {
        get { return input; }
    }
    public string DestinationName
    {
        get { return lastDestinationName; }
    }
    public GameObject PlayerPrefab
    {
        get { return playerPrefab; }
    }
    public GameObject PlayControllerPrefab
    {
        get { return playControllerPrefab; }
    }

    public GameObject GameplayUIPrefab
    {
        get { return gameplayUIPrefab; }
    }

    void Start()
	{
        GameplayEventManager.OnSceneChange += SceneDestination;
        DontDestroyOnLoad(this.gameObject);
        input = GetComponent<InputController>() != null ? GetComponent<InputController>() : gameObject.AddComponent<InputController>();
        input.enabled = false;
		ChangeState<MainMenuState>();
	}

    void SceneDestination(Object sender, int sceneIndex, string destination)
    {
        lastDestinationName = destination;
    }
}
