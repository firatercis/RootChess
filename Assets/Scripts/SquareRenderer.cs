using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRenderer : MonoBehaviour
{

    // Settings

    // Connections
    Renderer matRenderer;
    // State variables

    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        matRenderer = GetComponent<Renderer>();
    }

    void InitState(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color col)
    {
        if(matRenderer == null)
            matRenderer = GetComponent<Renderer>();
        matRenderer.material.color = col;
    }

    public void SetPosition (Vector3 position)
    {
        transform.position = position;
    }

}
