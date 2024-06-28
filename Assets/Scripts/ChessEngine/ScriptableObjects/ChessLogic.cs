using SoftwareKingdom.Chess.Core;
using System;

using System.Collections.Generic;
using UnityEngine;



namespace SoftwareKingdom.Chess.Core
{

    public struct GameResult {
        public int winningIndex; //0: Tie, 1: White -1: Black 
        public GameResult(int winningIndex) {
            this.winningIndex = winningIndex;   
        }
    }


    public abstract class ChessLogic : ScriptableObject, IChessLogic
    {
        // Events
        public  Action<Move> OnMovePlayed;
        public  Action<int> OnTurn;
        public  Action<GameResult> OnGameEnd;

        // State variables
        public bool gameEnd;
        protected ChessState currentState;
        
        public void PlayMove(ChessState inputState, Move move) {

            ApplyMove(inputState, move);
            
           

            OnMovePlayed?.Invoke(move);
            if(!gameEnd)
            {
                CheckGameEnd(inputState);
                OnTurn?.Invoke(inputState.turn);
            }
        }

        public ChessState GetState() {
            return currentState;    
        }

        public abstract void ApplyMove(ChessState boardState, Move move);

        public abstract List<Move> GenerateBoardMoves(ChessState boardState);

        public abstract List<Move> GenerateBoardMovesForOpponent(ChessState boardState);

        public abstract ChessState GenerateMoveSuccessor(ChessState inputState, Move move);


        public abstract List<Move> GenerateMoves(ChessState boardState, Coord sourceCoord,bool checkLegal = true);

        public abstract void CheckGameEnd(ChessState boardState);


        // For simplicity, use current state as default input
        public List<Move> GenerateBoardMoves() {
            return GenerateBoardMoves(currentState);
        }

        public List<Move> GenerateMoves(Coord sourceCoord) {
            return GenerateMoves(currentState, sourceCoord);
        }

        public void PlayMove( Move move) {
            PlayMove(currentState,move);
        }

        public virtual void StartGame() {
            gameEnd = false;
            OnTurn?.Invoke(currentState.turn);
        }

        public abstract string GetVariantName();
        public abstract ChessState CreateGame();

        protected virtual void SwitchTurn(ChessState boardState) {
            boardState.turn = (boardState.turn + 1) % ChessState.N_SIDES;
        }

       

       
    }

}

