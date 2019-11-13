using UnityEngine;

public class CursorAnimator : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairSprite;
    [SerializeField] private Texture2D cursorSprite;

    public void Awake()
    {
        Vector2 hotspot = new Vector2(cursorSprite.width / 2, cursorSprite.height / 2);
        Cursor.SetCursor(cursorSprite, hotspot, CursorMode.Auto);
    }
}
