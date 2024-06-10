using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PieceSelectionUI : MonoBehaviour
{

    // Settings

    // Connections
    public Image[] whiteButtonSprites;
    public Image[] blackButtonSprites;

    public GameObject pawnButtonGO;
    public GameObject kingButtonGO;

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

    public void Configure(bool isWhite, bool isSeed)
    {
        for(int i=0; i< whiteButtonSprites.Length; i++)
        {
            whiteButtonSprites[i].enabled = isWhite;
            blackButtonSprites[i].enabled = !isWhite;
        }

        kingButtonGO.SetActive(false); // TODO: King button
        pawnButtonGO.SetActive(isSeed); 
    }
}
