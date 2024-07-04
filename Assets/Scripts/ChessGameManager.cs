using SoftwareKingdom.Chess;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using DG.Tweening;



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
        //DOTween.SetTweensCapacity(3000, 1500); // İlk parametre aktif tweens, ikinci parametre sıralı tweens sayısını belirler

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
        logic.OnGameEnd += OnGameEnd;
    }


    void OnGameEnd(GameResult gameResult) {
        uiManager.OnGameEnd(gameResult);
        //Invoke(nameof(RestartGame), 5.0f);
    }

    void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void InitState(){
        currentBoardState = logic.CreateGame();
        uiManager.CreateBoard(currentBoardState); // TODO: For only test
        seedSelectionPanel.SetSeedsDisplay(new string[] { "P", "N", "B", "R", "Q" }); // TODO: Get Available seeds 
        Invoke(nameof(OnTurn), 1f);
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
        //uiManager.MakeMove(move, currentBoardState);
        if(!logic.gameEnd)
            Invoke(nameof(OnTurn), 0.01f);
    }
   

}
