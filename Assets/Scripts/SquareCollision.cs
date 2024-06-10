using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCollision : MonoBehaviour
{

    // Settings
    public int rankIndex;
    public int fileIndex;
    // Connections

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

    private void OnMouseDown()
    {
        Debug.Log("Clicked ");
    }
}
