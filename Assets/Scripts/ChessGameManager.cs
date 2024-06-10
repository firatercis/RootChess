using SoftwareKingdom.Chess;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessGameManager : MonoBehaviour
{

    // Settings

    // Connections
    public ChessLogic logic;

    public ChessPlayer[] players;

    public BaseChessUI uiManager;
    public SeedSelectionPanel seedSelectionPanel;

    
    // State variables
    ChessState currentBoardState;
    void Awake(){
        InitConnections();
    }

    void Start()
    {
        InitState();
    }

    void InitConnections(){
        // Configure Players
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Configure(logic);
        }
        logic.OnMovePlayed += OnMovePlayed;

    }

    void InitState(){
        currentBoardState = logic.CreateGame();
        uiManager.CreateBoard(currentBoardState); // TODO: For only test
        seedSelectionPanel.SetSeedsDisplay(new string[] { "P", "N", "B", "R", "Q" }); // TODO: Get Available seeds 
        OnTurn();
    }

    void OnTurn() {
        players[currentBoardState.turn].OnTurn(currentBoardState);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMoveSelected(Move move)
    {

    }

    public void OnMovePlayed(Move move)
    {
      //  uiManager.MakeMove(move, currentBoardState);
        OnTurn();
    }
   
}
