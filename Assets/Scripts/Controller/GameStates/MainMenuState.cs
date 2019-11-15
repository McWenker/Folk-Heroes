using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : GameState
{
    public override void Enter()
	{
		base.Enter ();
		StartCoroutine(MenuInit());
	}

    protected override void AddListeners()
    {
        MenuEventManager.OnButtonPress += MenuButton;
    }

    protected override void RemoveListeners()
    {
        MenuEventManager.OnButtonPress -= MenuButton;
    }

    IEnumerator MenuInit()
    {
        yield return null;
        SceneManager.LoadScene(0);
    }

    void MenuButton(Object sender)
    {
        if(sender.GetType() == typeof(MainMenuButton))
            owner.ChangeState<InitGameState>();
    }
}
