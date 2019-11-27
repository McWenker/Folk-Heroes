using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractable : Interactable
{  
    [SerializeField] GameObject objectToToggle;

    public override void Interact()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
