using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D reticleSprite;

    private void Start()
    {
        if (reticleSprite != null)
            Cursor.SetCursor(reticleSprite, Vector2.zero, CursorMode.Auto);
    }
}
