using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color outlineColor = Color.white;
    public bool outlineOn;
    public bool lerpColor;
    public bool flashActive;
    public int flashCount = 3;
    private bool flashOn = true;
    private int flashCounter = 0;

    private SpriteRenderer spriteRenderer;

    void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(true);
    }

    void OnDisable() {
        UpdateOutline(false);
    }

    void Update() {
        if(lerpColor)
        {
            outlineColor = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1));
        }
        if(flashActive)
        {
            if(flashOn && flashCounter <= flashCount)
            {
                UpdateOutline(false);
                flashCounter++;
                return;
            }
            else if(!flashOn && flashCounter <= flashCount)
            {
                flashCounter++;
            }
            else if(flashCounter >= flashCount)
            {
                flashCounter = 0;
                flashOn = !flashOn;
            }
        }
        UpdateOutline(outlineOn);      
        
    }

    void UpdateOutline(bool outline) {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", outlineColor);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}