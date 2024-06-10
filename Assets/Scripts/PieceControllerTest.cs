using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceControllerTest : MonoBehaviour
{

    // Settings
    int sproutTime = 9;
    // Connections
    [Required]
    public PieceController pieceController;
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

    [Button("Start Sprout")]
    public void StartSprout()
    {
        pieceController.Sprout(sproutTime);
    }

    [Button("Raise 1 Step")]
    public void RaiseStep()
    {
        pieceController.RaiseStep();
    }



}
