
using System;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftwareKingdom.Chess.UI
{

    [CreateAssetMenu(fileName = "AIPlayer2", menuName = "SoftwareKingdom/Chess/AIPlayer2", order = 2)]

    public class AIPlayer2 : ChessPlayer {

        // Settings

        // Connections
        public ChessLogic gameLogic;
        ChessState currentChessState;
    
        public override bool IsHuman() { return false; }

        public override void Configure(ChessLogic gameLogic){
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {
            
            gameLogic.PlayMove(state, GetBestMoveOfAll(state));
        }
        
        private double EvaluateBoard(ChessState state){
            double point = GetBoardPieceStatus(state);
            return point;
        }

        private double MiniMax(ChessState state, double alpha, double beta, int depth, int maximizingPlayer){

            if(depth == 0){
                
                return EvaluateBoard(state);
            }

            else{
                if(maximizingPlayer == 0){
                    double bestValue = -100;
                    List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);

                    for(int i=0; i<possibleMoves.Count; i++){

                        ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                        double value = MiniMax(successorState, alpha, beta, depth-1, 0);
                        if(value > bestValue){
                            bestValue = value;
                            Debug.Log(bestValue + "i");
                        }
                        if(value > alpha)
                            alpha = value;
                        if(beta <= alpha)
                            break;
                    }
                    return bestValue;
                }
                else{
                    double bestValue = 100;
                    List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);

                    for(int i=0; i<possibleMoves.Count; i++){

                        ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                        double value = MiniMax(successorState, alpha, beta,depth-1, 1);
                        if(value < bestValue)
                            bestValue = value;
                        if(value < beta)
                            beta = value;
                        if(beta <= alpha)
                            break;
                    }
                    return bestValue;
                }

            }
        }



        private Move GetBestMove(ChessState state){

            int turnPrefix = state.turn;
            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            
            int randomNumber = GenerateRandomNumber(possibleMoves.Count);
            Move bestMove = possibleMoves[randomNumber];
            ChessState tempState = gameLogic.GenerateMoveSuccessor(state, bestMove);

            double bestPointControl = EvaluateBoard(tempState);
            double bestPoint = EvaluateBoard(tempState);
            double tempPoint;

            for(int i = 0; i<possibleMoves.Count; i++){
                ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                tempPoint = EvaluateBoard(successorState);

                if(turnPrefix == 0){
                    if(tempPoint >= bestPoint){
                        bestPoint = tempPoint;
                        bestMove = possibleMoves[i];
                    }
                }
                if(turnPrefix == 1){
                    if(tempPoint <= bestPoint){
                        bestPoint = tempPoint;
                        bestMove = possibleMoves[i];
                    }
                }
            }
            if(turnPrefix == 0 && bestPoint <= bestPointControl){
                randomNumber = GenerateRandomNumber(possibleMoves.Count);
                bestMove = possibleMoves[randomNumber];
            }
            if(turnPrefix == 1 && bestPoint >= bestPointControl){
                randomNumber = GenerateRandomNumber(possibleMoves.Count);
                bestMove = possibleMoves[randomNumber];
            }

            return bestMove;
        }

        private Move GetBestMoveOfAll(ChessState state){

            int turnPrefix = state.turn;
            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            
            int randomNumber = GenerateRandomNumber(possibleMoves.Count);
            Move bestMove = possibleMoves[randomNumber];
            ChessState tempState = gameLogic.GenerateMoveSuccessor(state, bestMove);


            double bestPointControl = EvaluateBoard(tempState);
            double bestPoint = EvaluateBoard(tempState);

            double bestValue = 0;
            double minAlpha = -99;
            double maxBeta = 99;


            for(int i = 0; i<possibleMoves.Count; i++){
                ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                if(turnPrefix == 0){
                    double tempValue = MiniMax(successorState, minAlpha, maxBeta, 1, 0);
                    if(tempValue > bestPoint){ 
                        bestPoint = tempValue;
                        bestMove = possibleMoves[i];
                        Debug.Log(bestPoint);
                    }
                }
                if(turnPrefix == 1){
                    double tempValue = MiniMax(successorState, minAlpha, maxBeta, 1, 1);
                    if(tempValue < bestPoint){
                        bestPoint = tempValue;
                        bestMove = possibleMoves[i];
                    }
                }
            }
            
            if(turnPrefix == 0 && bestPoint <= bestPointControl){
                randomNumber = GenerateRandomNumber(possibleMoves.Count);
                bestMove = possibleMoves[randomNumber];
            }
            if(turnPrefix == 1 && bestPoint >= bestPointControl){
                randomNumber = GenerateRandomNumber(possibleMoves.Count);
                bestMove = possibleMoves[randomNumber];
            }
            

            return bestMove;
        }

        private int GenerateRandomNumber(int numberOfMoves){

            System.Random random = new System.Random();

            // Üretmek istediğiniz sayı aralığını belirleyin
            int maxNumber = numberOfMoves; 

            // Rastgele bir sayı üretin (0 dahil, maxNumber hariç)
            int randomNumber = random.Next(0, maxNumber);

            return randomNumber;
        }
        
        private double GetBoardPieceStatus(ChessState boardState){
            double score = 0;
                for (int i = 0; i < boardState.board.GetLength(0); i++){
                    for (int j = 0; j < boardState.board.GetLength(1); j++){

                        if(boardState.IsEmpty(i, j)) continue;
                        string pieceNotation = boardState.board[i,j];

                        if(pieceNotation == "WP") score += 2;          
                        if(pieceNotation == "WR") score += 5; 
                        if(pieceNotation == "WN") score += 3;           
                        if(pieceNotation == "WB") score += 3;                   
                        if(pieceNotation == "WQ") score += 9;
                        if(pieceNotation == "WK") score += 21;
                        
                        if(pieceNotation == "BP") score -= 2;
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