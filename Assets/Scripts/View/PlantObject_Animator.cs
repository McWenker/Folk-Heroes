using UnityEngine;

public class PlantObject_Animator : MonoBehaviour
{
    [SerializeField] SpriteAnimator animator;
    
    public void SpriteChange(Sprite toSet)
    {
        animator.SetSprite(transform, toSet);
    }
}
