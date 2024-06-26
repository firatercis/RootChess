
using System;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftwareKingdom.Chess.UI
{

    [CreateAssetMenu(fileName = "AIPlayer1", menuName = "SoftwareKingdom/Chess/AIPlayer1", order = 2)]

    public class AIPlayer1 : ChessPlayer 
    {

        // Settings

        // Connections
        public ChessLogic gameLogic;
        ChessState currentChessState;
    
        public override bool IsHuman() { return false; }

        public override void Configure(ChessLogic gameLogic){
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {
            
            int bestMoveWhite = -9999;
            int bestMoveBlack = +9999;
            Move bestMove = new Move(new Coord(0,0), new Coord(0,0));

            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            //Move move = possibleMoves[0];
            for(int k =0; k < possibleMoves.Count; k++){

                ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[k]);

                int currentBest = 0;
                for (int i = 0; i < successorState.board.GetLength(0); i++){
                    for (int j = 0; j < successorState.board.GetLength(1); j++){

                        if(successorState.IsEmpty(i, j)) continue;
                        string pieceNotation = successorState.board[i,j];

                        if(pieceNotation == "WP") currentBest += 1;
                        if(pieceNotation == "WR") currentBest += 5;
                        if(pieceNotation == "WN") currentBest += 3;
                        if(pieceNotation == "WB") currentBest += 3;
                        if(pieceNotation == "WQ") currentBest += 9;
                        if(pieceNotation == "WK") currentBest += 21;

                        if(pieceNotation == "BP") currentBest -= 1;
                        if(pieceNotation == "BR") currentBest -= 5;
                        if(pieceNotation == "BN") currentBest -= 3;
                        if(pieceNotation == "BB") currentBest -= 3;
                        if(pieceNotation == "BQ") currentBest -= 9;
                        if(pieceNotation == "BK") currentBest -= 21;


                    }
                }
                
                


                if(state.turn == 0){

                    if(currentBest > bestMoveWhite){
                        bestMoveWhite = currentBest;
                        bestMove = possibleMoves[k];
                    }
                            
                            
                }
                if(state.turn == 1){
                    if(currentBest < bestMoveBlack){
                        bestMoveBlack = currentBest;
                        bestMove = possibleMoves[k];
                    } 
                    
                }
            }

            if(possibleMoves.Count != 0){

                gameLogic.PlayMove(state, bestMove);
                
            }
        }

        private int GenerateRandomNumber(int numberOfMoves){

            System.Random random = new System.Random();

            // Üretmek istediğiniz sayı aralığını belirleyin
            int maxNumber = numberOfMoves; 

            // Rastgele bir sayı üretin (0 dahil, maxNumber hariç)
            int randomNumber = random.Next(0, maxNumber);

            return randomNumber;
        }
    }
}


