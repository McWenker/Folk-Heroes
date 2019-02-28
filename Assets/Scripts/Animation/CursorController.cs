using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairSprite;
    [SerializeField] private Texture2D menuSprite;

    public void UpdateState(ControlState state)
    {
        Texture2D cursor = null;
        switch(state)
        {
            case ControlState.Combat:
                cursor = crosshairSprite;
                break;
            case ControlState.Command:
                cursor = menuSprite;
                break;
            case ControlState.Menu:
                cursor = menuSprite;
                break;
            case ControlState.Construction:
                if(EventSystem.current.IsPointerOverGameObject())
                    cursor = menuSprite;
                break;
        }
        Cursor.visible = (cursor != null);
        if (cursor != null)
        {
            Vector2 hotspot = new Vector2(cursor.width / 2, cursor.height / 2);
            Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
        }
    }
    private void Awake()
    {
        UpdateState(ControlState.Combat);
    }
}
