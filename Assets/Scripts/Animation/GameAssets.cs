using System.Reflection;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    private void Awake()
    {
        if(GetComponent<GameAssets>() != false)
        {
            _i = GetComponent<GameAssets>();
        }
    }

    public Sprite goldNodeDepletedSprite;
    public Sprite ironNodeDepletedSprite;
    public Sprite manaNodeDepletedSprite;
}
