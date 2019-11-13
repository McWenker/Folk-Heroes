using UnityEngine;
[System.Serializable]
public class Triggers
{
    public string name;
    [SerializeField] public Sprite[] leftHandSprites;
    [SerializeField] public Sprite[] rightHandSprites;
    public int keyFrame;    
    public float cooldown;    
    public float range;
    [SerializeField] public Effect[] effects;
}
