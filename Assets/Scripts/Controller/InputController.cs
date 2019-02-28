using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    KeyCode[] _abilityButtons = new KeyCode[]{KeyCode.Q, KeyCode.E, KeyCode.Space, KeyCode.R, KeyCode.F};
    KeyCode[] _controlStateButtons = new KeyCode[]{KeyCode.LeftAlt, KeyCode.RightAlt};

    void HandleControlState()
    {
        for(int i = 0; i < _controlStateButtons.Length; ++i)
        {
            if(Input.GetKey(_controlStateButtons[i]))
            {
                InputEventManager.ControlStateChange(this);
            }
        }
    }
	void HandleMovement()
	{
		float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        Vector3 baseMoveDir = new Vector3(moveX, 0, moveY).normalized;
		InputEventManager.Move(this, baseMoveDir);
	}

    void HandleAbilities()
    {
        for(int i = 0; i < _abilityButtons.Length; ++i)
        {
            if(Input.GetKey(_abilityButtons[i]))
            {
                //Debug.Log(_abilityButtons[i].ToString());
                InputEventManager.AbilityUse(this, _abilityButtons[i].ToString());
            }
        }
    }

    void HandleMouse()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetMouseButtonDown(0))
            {
                InputEventManager.MouseDown(this, 0);
            }
            if(Input.GetMouseButtonUp(0))
            {
                InputEventManager.MouseUp(this, 0);
            }
            if(Input.GetMouseButton(0))
            {   
                InputEventManager.Fire(this, 0);
            }
            else if(Input.GetMouseButton(1))
            {
                InputEventManager.Fire(this, 1);
            }
        }    
        else
        {
            InputEventManager.MouseUp(this, 0);
        }    
    }
	void Update()
	{
        HandleControlState();
		HandleMovement();
        HandleAbilities();
        HandleMouse();
	}
}
