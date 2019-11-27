using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        GameplayEventManager.OnSceneChangeParams += SceneChange;
        SceneManager.sceneLoaded += AfterChange;
        animator = GetComponent<Animator>();
        StartCoroutine(TransitionThenPlay());
    }

    private void SceneChange(Object sender, int sceneIndex, string destination)
    {        
        StartCoroutine(LoadSceneAfterTransition(sceneIndex));
    }

    private void AfterChange(Scene loaded, LoadSceneMode mode)
    {
        if(loaded.buildIndex != 0 && this != null)
            StartCoroutine(TransitionThenPlay());
    }

    private IEnumerator LoadSceneAfterTransition(int toChange)
    {        
        //show animate out animation
        animator.SetBool("animateOut", true);
        yield return new WaitForSeconds(1f);
        //load the scene we want
        SceneManager.LoadScene(toChange);
    }

    private IEnumerator TransitionThenPlay()
    {
        animator.SetBool("animateOut", false);
        animator.SetBool("animateIn", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("animateIn", false);
        GameplayEventManager.SceneTransitionFinished(this);
    }
}
