using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface SquareClickListener
{
    void OnSquareClick(int rankIndex, int fileIndex);
}

public class BoardSquare : MonoBehaviour
{

    // Settings

    // Connections
    public SpriteRenderer highlightSprite;
    Renderer squareRenderer;
    public Transform pieceNest;
    
    SquareClickListener listener;
    // State variables
    int rankIndex;
    int fileIndex;

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

    public void Configure(int rankIndex, int fileIndex, Color color, Vector3 position, SquareClickListener listener = null)
    {
        if(squareRenderer == null) squareRenderer = GetComponent<Renderer>();
        this.rankIndex = rankIndex;
        this.fileIndex = fileIndex;
        gameObject.SetActive(true);
        transform.position = position;
        squareRenderer.material.color = color;
        this.listener = listener;
    }
    public void OnMouseDown()
    {
        Debug.Log("Clicked square: " + fileIndex + " , " + rankIndex);
        listener?.OnSquareClick(rankIndex, fileIndex);
    }

    public void Highlight()
    {
        highlightSprite.gameObject.SetActive(true);
    }

    public void UnHighlight()
    {
        highlightSprite.gameObject.SetActive(false);
    }

}
