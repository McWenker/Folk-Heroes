using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayEventManager
{
    //public delegate void NPCEvent(Object sender, NPC npc);
    public delegate void SceneTransitionEvent(Object sender);
    public delegate void SceneChangeEvent(Object sender, int sceneIndex, string destination);
    public static event SceneTransitionEvent OnSceneTransitionFinish;
    public static event SceneChangeEvent OnSceneChange;

    public static void SceneTransitionFinished(Object sender)
    {
        if(OnSceneTransitionFinish != null) OnSceneTransitionFinish(sender);
    }

    public static void SceneChange(Object sender, int sceneIndex, string destination)
    {
        if(OnSceneChange != null) OnSceneChange(sender, sceneIndex, destination);
    }
    //public static event NPCEvent OnNPCSpeak;

    /*public static void NPCSpeak(Object sender, NPC npc)
    {
        if(OnNPCSpeak != null) OnNPCSpeak(sender, npc);
    }*/


}