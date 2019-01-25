using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStateHandler : MonoBehaviour
{
    [SerializeField] ConstructionHandler constructionHandler;
    [SerializeField] CanvasRenderer constructionCanvas;
    [SerializeField] CursorController cursorController;

    private ControlState SwapState(GameObject sender, ControlState state)
    {
        ControlState retVal;
        if(state == ControlState.Combat)
            retVal = ControlState.Menu;
        else if(state == ControlState.Menu || state == ControlState.Construction)
            retVal = ControlState.Combat;
        else
            retVal = state;
        SetState(retVal);
        return retVal;
    }

    private void Awake()
    {
        ControlEventManager.OnControlStateSwap += SwapState;
        ControlEventManager.OnControlStateSet += SetState;
    }

    private void UpdateConstruction(ControlState state)
    {
        constructionCanvas.gameObject.SetActive(state == ControlState.Menu || state == ControlState.Construction);
        constructionHandler.enabled = (state == ControlState.Construction);
    }

    private void SetState(Object sender, ControlState state)
    {
        if(sender != this)
        {
            SetState(state);
        }
    }

    private void SetState(ControlState state)
    {
        UpdateConstruction(state);
        cursorController.UpdateState(state);
    }

    public void ConstructionState()
    {
        SetState(ControlState.Construction);
    }

    public void ExitConstruction()
    {
        SetState(ControlState.Combat);
    }
}
