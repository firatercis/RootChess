using SoftwareKingdom.Chess.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace SoftwareKingdom.Chess
{



    public abstract class ChessLogic : ScriptableObject, IChessLogic
    {
        // Events
        public event Action<Move> OnMovePlayed;
        public event Action<int> OnTurn;
        // State variables

        protected ChessState currentState;
        
        public void PlayMove(ChessState inputState, Move move) {
            ApplyMove(inputState, move);
            
            OnMovePlayed?.Invoke(move);
            OnTurn?.Invoke(inputState.turn);
        }

        public ChessState GetState() {
            return currentState;    
        }

        public abstract void ApplyMove(ChessState boardState, Move move);

        public abstract List<Move> GenerateBoardMoves(ChessState boardState);

        public abstract List<Move> GenerateMoves(ChessState boardState, Coord sourceCoord,bool checkLegal = true);

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
            OnTurn?.Invoke(currentState.turn);
        }

        public abstract string GetVariantName();
        public abstract ChessState CreateGame();

        protected virtual void SwitchTurn(ChessState boardState) {
            boardState.turn = (boardState.turn + 1) % ChessState.N_SIDES;
        }

       

       
    }

}

