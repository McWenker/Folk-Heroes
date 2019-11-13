using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
    public Animator animator;
    public Transform Xsway;
    public float sway;
    private Text damageText;
    private Vector3 spawnPosition;
    private Vector3 xSwayTarget;
    private float clipLength;
    private float clipRemaining;

    public Vector3 Position
    {
        set
        {
            transform.position = Camera.main.WorldToScreenPoint(value);
            xSwayTarget = new Vector3(Random.Range(Xsway.localPosition.x - sway, Xsway.localPosition.x + sway), Xsway.localPosition.y, Xsway.localPosition.z);
            spawnPosition = value;
        }
        get
        {
            return spawnPosition;
        }
    }

    void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        clipLength = clipInfo[0].clip.length;
        clipRemaining = 0;
        Destroy(gameObject, clipLength);
        damageText = animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        damageText.text = text;
    }

    public void SetText(int number)
    {
        damageText.text = (number.ToString());
    }

    void FixedUpdate()
    {
        if(isActiveAndEnabled && this != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(spawnPosition);
            Xsway.localPosition = Vector3.Lerp(Xsway.localPosition, xSwayTarget, clipRemaining/clipLength);
            clipRemaining += Time.fixedDeltaTime;
        }
    }
}