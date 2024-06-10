using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PieceController : MonoBehaviour
{

    // Settings
    public float pieceRaiseWaitTime = 2.0f;
    public float raiseStepTime = 1.0f;
    public float raisingZRotation = 10.0f;
    //public float pieceInitialScale;
    // Connections
    public SpriteRenderer pieceAppearence;
    public Animator sproutingAnimator;
    public SpriteRenderer raisingPiece;
    public Transform pieceInitialRaisePoint;
    public Transform pieceFullRaisePoint;
    public GameObject earthMask;
    public GameObject remainingMovesTextParent;
    public TMP_Text remainingMovesText;
    // State variables
    Sprite mainSprite;
    int nRaiseLevels;
    int currentRemainingStages;
    Vector3 initialRotation;
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
        initialRotation = raisingPiece.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetMainSprite(Sprite sprite)
    {
        mainSprite = sprite;
        pieceAppearence.sprite = mainSprite;
        raisingPiece.sprite = mainSprite;
    }

    public void Sprout(int nRaiseLevels )
    {
        this.nRaiseLevels = nRaiseLevels;
        currentRemainingStages = nRaiseLevels;
        PlayInitialSprouting();
        Invoke(nameof(StartRaising), pieceRaiseWaitTime); // TODO: Magic number for time
    }
    
    public void StartRaising()
    {
        earthMask.SetActive(true);
        sproutingAnimator.gameObject.SetActive(false);
        raisingPiece.gameObject.SetActive(true);
        SetGrowth(nRaiseLevels);


    }

    public void RaiseStep()
    {
        //remainingMovesText.text = currentRemainingStages.ToString();
        currentRemainingStages--;
        SetGrowth(currentRemainingStages);

        //if(currentRemainingStages > 0)
        //{
        //    float growthRate = (float)(nRaiseLevels - currentRemainingStages) / nRaiseLevels;
        //    float targetHeight = Mathf.Lerp(pieceInitialRaisePoint.transform.position.y, pieceFullRaisePoint.position.y, growthRate);
        //    raisingPiece.transform.DOMoveY(targetHeight, raiseStepTime);
        //}
        //else
        //{
        //    FullyGrow();
        //}

        //raisingPiece.transform.DORotate(initialRotation + (Vector3.forward * raisingZRotation), raiseStepTime);
        //raisingZRotation = -raisingZRotation;

    }

    public void SetGrowth(int remainingStages)
    {
        remainingMovesText.text = remainingStages.ToString();

        if (remainingStages > 0)
        {
            float growthRate = (float)(nRaiseLevels + 1 - remainingStages) / nRaiseLevels; // +1 for the first raise
            float targetHeight = Mathf.Lerp(pieceInitialRaisePoint.transform.position.y, pieceFullRaisePoint.position.y, growthRate);
            raisingPiece.transform.DOMoveY(targetHeight, raiseStepTime);
        }
        else
        {
            FullyGrow();
        }

        raisingPiece.transform.DORotate(initialRotation + (Vector3.forward * raisingZRotation), raiseStepTime);
        raisingZRotation = -raisingZRotation;

    }

    void FullyGrow()
    {
        SetPieceMode();
        pieceAppearence.transform.DOPunchScale(0.2f * Vector3.one, 0.5f, vibrato: 0);


    }

    public void SetPieceMode()
    {
        pieceAppearence.gameObject.SetActive(true);
        sproutingAnimator.gameObject.SetActive(false);
        remainingMovesTextParent.SetActive(false);
        raisingPiece.gameObject.SetActive(false);
        earthMask.SetActive(false);
    }

    public void PlayInitialSprouting()
    {
        pieceAppearence.gameObject.SetActive(false);
        sproutingAnimator.gameObject.SetActive(true);
        remainingMovesTextParent.SetActive(true);
        sproutingAnimator.SetTrigger("Sprout");
    }

}
