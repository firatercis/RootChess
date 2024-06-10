using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSprouting : MonoBehaviour
{

    // Settings
    public int maxStage = 9;
    public float growthTime = 1.0f;
    // Connections
    public SproutController sproutController;

    // State variables
    int currentStage;
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        sproutController = GetComponent<SproutController>();
    }

    void InitState(){
        currentStage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentStage++;
            float growthPercent = (float) currentStage / (float)maxStage;
            sproutController.SetGrowth(growthPercent,growthTime);
        }
    }
}
