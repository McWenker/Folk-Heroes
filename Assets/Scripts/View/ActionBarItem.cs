using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarItem : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Image icon;

    public void Color(Color toSet)
    {
        background.color = toSet;
    }

    public void Set(Sprite imageToSet)
    {
        icon.sprite = imageToSet;
        icon.enabled = imageToSet == null ? false : true;
    }
}
