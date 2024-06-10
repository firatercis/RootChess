using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLeaf : MonoBehaviour
{
    const float KEY_VALUE_MAX = 100;

    // Settings

    // Connections
    public AnimationCurve[] growingAnimationCurves;
    public SkinnedMeshRenderer leafRenderer;
    // State variables

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGrowth(float growthPercent)
    {
        for (int keyIndex = 0; keyIndex < growingAnimationCurves.Length; keyIndex++)
        {
            float keyValue = growingAnimationCurves[keyIndex].Evaluate(growthPercent) * KEY_VALUE_MAX;
            leafRenderer.SetBlendShapeWeight(keyIndex, keyValue);
        }
    }

}
