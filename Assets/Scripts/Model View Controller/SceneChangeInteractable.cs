using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeInteractable : Interactable
{
    SceneChanger changer;
    public override void Interact()
    {
        changer.ChangeScene();
    }

    void Awake()
    {
        changer = GetComponent<SceneChanger>();
    }
}
