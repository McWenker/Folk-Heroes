using UnityEngine;

[System.Serializable]
public class PlantTiers
{
    public string name;
    public Sprite tierSprite;
    public Sprite deathSprite;
    //public PlantProduct tierProduct;
    public bool overflowStays;
    public int overflowFactor;
    public int growthThresh;
    public int deathThresh;
    public int stagnantDaysThresh;
}
