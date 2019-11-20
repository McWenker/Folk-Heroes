using UnityEngine;

public class PlantObject_Animator : WorldObject_Animator
{    
    public void SpriteChange(Sprite toSet)
    {
        anim.SetSprite(transform, toSet);
    }
}
