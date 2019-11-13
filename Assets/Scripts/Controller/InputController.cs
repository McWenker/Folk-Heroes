using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    KeyCode[] _abilityButtons = new KeyCode[]{KeyCode.Q, KeyCode.E, KeyCode.Space, KeyCode.R, KeyCode.F};
    KeyCode[] _actionBarButtons = new KeyCode[]{KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0};
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

    void HandleEquipmentSwaps()
    {
        for(int i = 0; i < _actionBarButtons.Length; ++i)
        {
            if(Input.GetKey(_actionBarButtons[i]))
            {
                InputEventManager.ActionBarPress(this, i);
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
                InputEventManager.MouseUp(this, 1);
            }
            if(Input.GetMouseButtonUp(0))
            {
                InputEventManager.MouseUp(this, 0);
            }
            if(Input.GetMouseButton(0))
            {   
                InputEventManager.MouseHold(this, 0);
            }
            if(Input.GetMouseButtonDown(1))
            {
                InputEventManager.MouseDown(this, 1);
                InputEventManager.MouseUp(this, 0);
            }
            if(Input.GetMouseButtonUp(1))
            {
                InputEventManager.MouseUp(this, 1);
            }
            if(Input.GetMouseButton(1))
            {   
                InputEventManager.MouseHold(this, 1);
            }

            if(Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                InputEventManager.MouseScroll(this, Input.GetAxis("Mouse ScrollWheel"));
            }
        }
    }
	void Update()
	{
        HandleControlState();
		HandleMovement();
        HandleAbilities();
        HandleEquipmentSwaps();
        HandleMouse();
	}
}
