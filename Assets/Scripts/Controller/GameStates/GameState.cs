using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : State
{
    protected GameController owner;
    protected virtual void Awake()
    {
        owner = GetComponent<GameController>();
    }
}