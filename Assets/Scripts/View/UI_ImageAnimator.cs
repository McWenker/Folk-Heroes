using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImageAnimator : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    Sprite originalSprite;
    [SerializeField] Image image;
    [SerializeField] bool isPlaying;
    float timer;
    [SerializeField] float frameRate;

    int currentFrame;

    [SerializeField] bool isLoop;
    private int maxLoopCount;
    private int loopCounter;

    public void PlayAnimation(bool isLoop)
    {
        if(image.enabled)
        {
            originalSprite = image.sprite;
            if(isPlaying)
            {
                return;
            }
            else
                isPlaying = true;
            this.isLoop = isLoop;
            this.maxLoopCount = 0;
            currentFrame = 0;
            timer = 0f;
            image.sprite = sprites[currentFrame];
        }
    }

    public void StopPlaying()
    {
        isPlaying = false;
        loopCounter = -1;
        image.sprite = originalSprite;
    }

    private void FixedUpdate()
    {
        if (!isPlaying)
            return;

        if (sprites.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % sprites.Length;

            if (!isLoop && currentFrame == 0)
                StopPlaying();
            else if (isLoop && maxLoopCount != 0 && loopCounter >= maxLoopCount)
                StopPlaying();
            else
                image.sprite = sprites[currentFrame];

            if (currentFrame == 0)
            {
                loopCounter++;
            }
        }
    }

}
