using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    [SerializeField] string destination;

    public void ChangeScene()
    {
        GameplayEventManager.SceneChange(this, sceneIndex, destination);
    }
}
