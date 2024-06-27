
using System;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftwareKingdom.Chess.UI
{

    [CreateAssetMenu(fileName = "AIPlayer1", menuName = "SoftwareKingdom/Chess/AIPlayer1", order = 2)]

    public class AIPlayer1 : ChessPlayer {

        // Settings

        // Connections
        public ChessLogic gameLogic;
        ChessState currentChessState;
    
        public override bool IsHuman() { return false; }

        public override void Configure(ChessLogic gameLogic){
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {
            
            int bestMoveWhite = -300;
            int bestMoveBlack = 300;

            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            Move bestMove = new Move(new Coord(99,0), new Coord(0,99));
            if(possibleMoves.Count != 0){
            bestMove = possibleMoves[GenerateRandomNumber(possibleMoves.Count)];
            }

            //Move move = possibleMoves[0];
            for(int k =0; k < possibleMoves.Count; k++){

                ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[k]);
                
                int currentBest = 0;
                for (int i = 0; i < successorState.board.GetLength(0); i++){
                    for (int j = 0; j < successorState.board.GetLength(1); j++){

                        if(successorState.IsEmpty(i, j)) continue;
                        string pieceNotation = successorState.board[i,j];

                        if(pieceNotation == "WP") currentBest += 3;          
                        if(pieceNotation == "WR") currentBest += 5; 
                        if(pieceNotation == "WN") currentBest += 3;           
                        if(pieceNotation == "WB") currentBest += 3;                   
                        if(pieceNotation == "WQ") currentBest += 9;
                        if(pieceNotation == "WK") currentBest += 21;
                        
                        if(pieceNotation == "BP") currentBest -= 3;
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
                int checkLastPiece = CheckLastPiece(state);
                Debug.Log(checkLastPiece);
                if(checkLastPiece == 1){
                    int randomNumber = GenerateRandomNumber(possibleMoves.Count);
                    bestMove = possibleMoves[randomNumber];
             
                }
                if(state.turn == 0 && bestMoveWhite == GetBoardStatus(state)){
                        
                    int randomNumber = GenerateRandomNumber(possibleMoves.Count);
                    bestMove = possibleMoves[randomNumber];
                    }
                if(state.turn == 1 && bestMoveBlack == GetBoardStatus(state)){
                        
                    int randomNumber = GenerateRandomNumber(possibleMoves.Count);
                    bestMove = possibleMoves[randomNumber];
                }


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

        private int CheckLastPiece(ChessState state){
                int checkLastPieceWhite = 0;
                int checkLastPieceBlack = 0;

                for(int i = 0; i < state.board.GetLength(0); i++){
                    for (int j = 0; j < state.board.GetLength(1); j++){

                        if(state.IsEmpty(i, j)) continue;
                        string pieceNotation = state.board[i,j];

                        if(pieceNotation == "WP"){
                            checkLastPieceWhite++;
                        } 
                        if(pieceNotation == "WR"){
                            checkLastPieceWhite++;
                        }
                        if(pieceNotation == "WN"){
                            checkLastPieceWhite++;
                        } 
                        if(pieceNotation == "WB"){
                            checkLastPieceWhite++;
                        } 
                        if(pieceNotation == "WQ"){
                            checkLastPieceWhite++;
                        } 
                        if(pieceNotation == "WK"){
                            checkLastPieceWhite++;
                        }


                        if(pieceNotation == "BP"){
                            checkLastPieceBlack++;
                        } 
                        if(pieceNotation == "BR"){
                            checkLastPieceBlack++;
                        }
                        if(pieceNotation == "BN"){
                            checkLastPieceBlack++;
                        } 
                        if(pieceNotation == "BB"){
                            checkLastPieceBlack++;
                        } 
                        if(pieceNotation == "BQ"){
                            checkLastPieceBlack++;
                        } 
                        if(pieceNotation == "BK"){
                            checkLastPieceBlack++;
                        }
                    }
                }
            if(state.turn == 0) return checkLastPieceBlack;
            if(state.turn == 1) return checkLastPieceWhite;

            return 0;

        }    

        private int GetBoardStatus(ChessState boardState){
            int score = 0;
                for (int i = 0; i < boardState.board.GetLength(0); i++){
                    for (int j = 0; j < boardState.board.GetLength(1); j++){

                        if(boardState.IsEmpty(i, j)) continue;
                        string pieceNotation = boardState.board[i,j];

                        if(pieceNotation == "WP") score += 3;          
                        if(pieceNotation == "WR") score += 5; 
                        if(pieceNotation == "WN") score += 3;           
                        if(pieceNotation == "WB") score += 3;                   
                        if(pieceNotation == "WQ") score += 9;
                        if(pieceNotation == "WK") score += 21;
                        
                        if(pieceNotation == "BP") score -= 3;
                        if(pieceNotation == "BR") score -= 5;
                        if(pieceNotation == "BN") score -= 3;
                        if(pieceNotation == "BB") score -= 3;
                        if(pieceNotation == "BQ") score -= 9;
                        if(pieceNotation == "BK") score -= 21;
                    }
                }
                return score;
        }
    }
  


}