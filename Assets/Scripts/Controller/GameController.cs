using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : StateMachine
{
    InputController input;
    public InputController Input
    {
        get { return input; }
    }
    void Start()
	{
        DontDestroyOnLoad(this.gameObject);
        input = GetComponent<InputController>() != null ? GetComponent<InputController>() : gameObject.AddComponent<InputController>();
        input.enabled = false;
		ChangeState<MainMenuState>();
	}
}
