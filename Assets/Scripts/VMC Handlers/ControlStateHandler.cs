using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStateHandler : MonoBehaviour
{
    [SerializeField] ConstructionHandler constructionHandler;
    [SerializeField] CanvasRenderer constructionCanvas;
    [SerializeField] CursorController cursorController;

    private ControlState controlState;
    private bool controlStateCooldown;
    public ControlState _ControlState
    {
        get { return controlState; }
    }

    /*private void Update()
    {
        Debug.Log(controlState);
    }*/
    private void Awake()
    {
        InputEventManager.OnControlStateChange += SwapState;
        InputEventManager.OnFire += ExitConstruction;
    }    

    private void ExitConstruction(Object sender, int buttonFired)
    {
        if(buttonFired == 1)
            if(controlState == ControlState.Construction)
                SetState(ControlState.Command);
    }
    private void SwapState(Object sender)
    {
        if(!controlStateCooldown)
        {
            controlStateCooldown = true;
            if(controlState == ControlState.Command || controlState == ControlState.Construction)
            {
                SetState(ControlState.Combat);
                StartCoroutine(ControlStateCooldown());
            }
            else if(controlState == ControlState.Combat)
            {
                SetState(ControlState.Command);
                StartCoroutine(ControlStateCooldown());
            }
        }        
    }

    private void UpdateConstruction()
    {
        constructionCanvas.gameObject.SetActive(controlState == ControlState.Command || controlState == ControlState.Construction);
        constructionHandler.enabled = (controlState == ControlState.Construction);
    }

    private void SetState(ControlState state)
    {
        controlState = state;
        UpdateConstruction();
        cursorController.UpdateState(state);
        EquipmentEventManager.ToggleWeapons(this, controlState == ControlState.Combat);
    }

    public void ConstructionState()
    {
        SetState(ControlState.Construction);
    }

    private IEnumerator ControlStateCooldown()
    {
        yield return new WaitForSeconds(1f);
        controlStateCooldown = false;
    }
}
