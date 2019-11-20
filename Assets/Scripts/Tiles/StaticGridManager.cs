using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticGridManager
{
    public static List <SceneGrid> sceneList;

    public static void AddScene(string sceneName, Dictionary<Vector3, WorldTile>[] tiles)
    {
        CheckOrInitSceneList();
        SceneGrid thisGrid = new SceneGrid(sceneName, tiles);
        sceneList.Add(thisGrid);
    }

    public static void UpdateScene(string sceneName, Dictionary<Vector3, WorldTile>[] tiles)
    {
        foreach(SceneGrid sg in sceneList)
        {
            if(sg.sceneName == sceneName)
                sg.Update(tiles);
        }
    }

    public static bool CheckScene(string sceneName)
    {
        if(CheckOrInitSceneList())
        {
            foreach(SceneGrid sg in sceneList)
            {
                if(sg.sceneName == sceneName)
                    return true;
            }
            return false;
        }
        return false;
    }

    public static Dictionary<Vector3, WorldTile>[] GetSceneDict(string sceneName)
    {
        if (CheckOrInitSceneList())
        {            
            foreach(SceneGrid sg in sceneList)
            {
                if(sg.sceneName == sceneName)
                {
                    return sg.Tiles;
                }
            }
            return null;
        }
        return null;
    }

    private static bool CheckOrInitSceneList()
    {
        if(sceneList == null)
        {
            sceneList = new List<SceneGrid>();
            return false;
        }
        return true;
    }
}
