using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SproutController : MonoBehaviour
{

    // Settings
    public float maxAnimTimeValue;
    // Connections
    public Animator animator;
    public SpriteRenderer targetSpriteRenderer;
    public Sprite[] animationFrames;
    // State variables
    float currentValue;
    float interframeTime;
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
    }

    void InitState(){
        currentValue = 0;
        interframeTime = maxAnimTimeValue / (animationFrames.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySprouting(int seedingStage, int maxStage)
    {
        if (seedingStage == 0) return;
        float growthStartPercent = (float)(seedingStage - 1) / (float)maxStage;
        float growthEndPercent = (float)seedingStage / (float)maxStage;

        int startFrameIndex = Mathf.FloorToInt( (float)(seedingStage - 1) * (float)animationFrames.Length / (float)maxStage);
        int endFrameIndex = Mathf.FloorToInt( (float)(seedingStage) * (float)animationFrames.Length / (float)maxStage);

        

        StartCoroutine(PlaySpriteAnim(startFrameIndex, endFrameIndex, null));
       // animator.Play("Sprouting",)
    }

    public void SetGrowth(float growthPercent, float growthTime)
    {

        SetGrowthFrame(growthPercent);


        //DOTween.To(
        //    () => currentValue,
        //    x => SetGrowthFrame(currentValue),
        //    growthPercent,
        //    growthTime
        //    );
    }

    public void SetGrowthFrame(float value)
    {
        currentValue = value;
        int frameIndex = GetFrameIndex(value);
        targetSpriteRenderer.sprite = animationFrames[frameIndex];
    }

    int GetFrameIndex(float growthPercent)
    {
        int frameIndex = Mathf.FloorToInt(growthPercent * (float)animationFrames.Length);
        if(frameIndex >= animationFrames.Length)
        {
            frameIndex = animationFrames.Length - 1;
        }
        return frameIndex;
    }

    IEnumerator PlaySpriteAnim(int startFrame, int endFrame, Action animEndEvent)
    {
        int nFrames = endFrame - startFrame + 1;
        endFrame = endFrame > animationFrames.Length - 1 ? animationFrames.Length - 1 : endFrame;
        for (int i=0; i<nFrames; i++)
        {
            targetSpriteRenderer.sprite = animationFrames[startFrame + i];
            yield return new WaitForSeconds(interframeTime);
        }
        animEndEvent?.Invoke();
    }


}
