using UnityEngine;

public class Projectile_Animator : MonoBehaviour
{
    [SerializeField] SpriteAnimator animator;

    public void BeginAnim(Sprite[] anim)
    {
        animator.PlayAnimation(anim, 0.05f, true);
    }

    public void DudAnim(Sprite[] dudAnim)
    {
        animator.PlayAnimation(dudAnim, 0.1f, 0, (() => {}), (() =>
        {            
            GameObject.Destroy(gameObject);
        }));
    }
}
