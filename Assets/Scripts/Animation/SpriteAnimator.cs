using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public event EventHandler OnAnimationLooped;

    [SerializeField] private Sprite[] frameArray;
    [SerializeField] private bool destroyOnComplete;
    private int currentFrame;
    private float timer;
    [SerializeField] private float frameRate = .1f;
    private SpriteRenderer spriteRenderer;
    private bool isLoop = false;
    private bool isTrigger = false;
    private bool isPlaying = true;
    private int triggerFrame;
    private int maxLoopCount;
    private int loopCounter;
    private Action onComplete;
    private Action onTrigger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        loopCounter = -1;
    }

    private void StopPlaying()
    {
        isPlaying = false;
        if (destroyOnComplete)
            Destroy(gameObject);
        if(onComplete != null)
            onComplete();
        loopCounter = -1;
        onComplete = null;
    }

    private void FixedUpdate()
    {
        if (!isPlaying)
            return;

        if (frameArray.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % frameArray.Length;

            if(isTrigger && currentFrame == triggerFrame)
                onTrigger();

            if (!isLoop && currentFrame == 0)
                StopPlaying();
            else if (isLoop && maxLoopCount != 0 && loopCounter >= maxLoopCount)
                StopPlaying();
            else
                spriteRenderer.sprite = frameArray[currentFrame];

            if (currentFrame == 0)
            {
               if (OnAnimationLooped != null)
                    OnAnimationLooped(this, EventArgs.Empty);
                loopCounter++;
            }
        }
    }

    public void PlayAnimation(Sprite[] frameArray, float frameRate, int triggerFrame, Action onTrigger, Action onComplete)
    {
        if (isPlaying)
        {
            if (this.frameArray.Equals(frameArray))
            {
                return;
            }
        }
        else
            isPlaying = true;

        loopCounter = -1;
        this.frameArray = frameArray;
        this.frameRate = frameRate;
        this.isTrigger = true;
        this.triggerFrame = triggerFrame;
        this.isLoop = false;
        this.maxLoopCount = 0;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frameArray[currentFrame];
        this.onTrigger = onTrigger;
        this.onComplete = onComplete;
    }

    public void PlayAnimation(Sprite[] frameArray, float frameRate, bool isLoop)
    {
        if(isPlaying)
        {
            if(this.frameArray.Equals(frameArray))
            {
                return;
            }
        }
        else
            isPlaying = true;

        this.frameArray = frameArray;
        this.frameRate = frameRate;
        this.isTrigger = false;
        this.isLoop = isLoop;
        this.maxLoopCount = 0;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frameArray[currentFrame];
    }

    public void PlayAnimation(Sprite[] frameArray, float frameRate, bool isLoop, int loopCount, Action onLastLoop)
    {
        if (isPlaying)
        {
            if (this.frameArray.Equals(frameArray))
            {
                return;
            }
        }
        else
            isPlaying = true;
        
        loopCounter = -1;
        this.frameArray = frameArray;
        this.frameRate = frameRate;
        this.isTrigger = false;
        this.isLoop = isLoop;
        this.maxLoopCount = loopCount;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frameArray[currentFrame];
        onComplete = onLastLoop;
    }
}
