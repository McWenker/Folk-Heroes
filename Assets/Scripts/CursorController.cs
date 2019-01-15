using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D reticleSprite;

    private void Start()
    {
        Vector2 hotspot = new Vector2(reticleSprite.width / 2, reticleSprite.height / 2);
        if (reticleSprite != null)
            Cursor.SetCursor(reticleSprite, hotspot, CursorMode.Auto);
    }
}
