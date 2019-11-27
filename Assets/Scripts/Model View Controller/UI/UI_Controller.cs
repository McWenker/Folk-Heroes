using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameplayEventManager.OnSceneChange += Hide;
        GameplayEventManager.OnSceneTransitionFinish += Show;
    }

    void Hide(Object sender)
    {
        canvas.enabled = false;
    }

    void Show(Object sender)
    {
        canvas.enabled = true;
    }
}
