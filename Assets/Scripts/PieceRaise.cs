using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceRaise : MonoBehaviour
{
    const float KEY_VALUE_MAX = 100;

    //public const string BLEND_SHAPE_KEY_CLOSED = "blendShape1.FlowerClosed1";
    //public const string BLEND_SHAPE_KEY_OPENED = "blendShape1.FlowerOpened1";

    // Settings
    public float beforeGrowthHeight;


    public AnimationCurve[] animationCurves;

    public float testGrowthSpeed = 0.3f;

    // Connections
    public FlowerLeaf[] leafs;
    public Transform[] raisingTransforms;
    public Transform pieceBody;
    // State variables

    float growth;
    float defaultHeight;
    Vector3 initialPieceScale;
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
        initialPieceScale = pieceBody.localScale;
        defaultHeight = leafs[0].transform.position.y;
        growth = 0;
        OnGrowth(growth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            growth += testGrowthSpeed * Time.deltaTime;
            growth = Mathf.Clamp(growth, 0, 1);
            OnGrowth(growth);
        }

        if (Input.GetKey(KeyCode.H))
        {
            growth -= testGrowthSpeed * Time.deltaTime;
            growth = Mathf.Clamp(growth, 0, 1);
            OnGrowth(growth);
        }

    }

    void OnGrowth(float growthPercent)
    {

        SetRaise(growthPercent);
        SetLeafsGrowth(growthPercent);
       
        SetBodyScale(growthPercent);
    }

    void SetLeafsGrowth(float value)
    {
        for(int i=0; i< leafs.Length; i++)
        {
            leafs[i].SetGrowth(value);
        }
    }

    void SetRaise(float percent)
    {
        float height = beforeGrowthHeight + (defaultHeight - beforeGrowthHeight) * percent;
        for(int i=0; i< raisingTransforms.Length; i++)
        {
            Vector3 tempPos = raisingTransforms[i].localPosition;
            tempPos.y = height;
            raisingTransforms[i].localPosition = tempPos;
        }
    }
    void SetBodyScale(float percent)
    {
        pieceBody.localScale = initialPieceScale * percent;
    }
}
