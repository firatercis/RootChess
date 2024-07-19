

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
        int kame = 0;

        // State variables
        Dictionary <string, double> transpositionTable = new Dictionary<string, double>();
        int numberOfTraversedNodes = 0;
    
        public override bool IsHuman() { return false; }

        public override void Configure(ChessLogic gameLogic){
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {
            kame = 0;

            gameLogic.PlayMove(state, GetBestMoveOfAll(state));
           
        }
        
        private double EvaluateBoard(ChessState state){
            double point = 0;
            point += GetBoardPieceStatus(state);
            //point += GetIfCenter(state);
            //point += EvaluateCheckForMove(state);
            //point += EvaluatePossibleCheckForMove(state);
            return point;
        }

        private double MiniMax(ChessState state, double alpha, double beta, int depth, int maximizingPlayer){
            //Debug.Log("Depth = " + depth);
            // Check transposition table
            double savedValue;

            numberOfTraversedNodes++;
            if (transpositionTable.TryGetValue(state.ToString(), out savedValue))
            {
                Debug.Log("Transposition Table works");
                return savedValue;
            }



            if (depth == 0){
                kame++;
                double value = EvaluateBoard(state);
             //   transpositionTable.Add(state.ToString(),value);   
                return value;
               
            }

            else{
                if(maximizingPlayer == 0){
                    //Debug.Log(state.turn + "aa");

                    double bestValue = -100;
                    List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);

                    for(int i=0; i<possibleMoves.Count; i++){

                        ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                        double value = MiniMax(successorState, alpha, beta, depth-1, 1);
                        if(value > bestValue){
                            bestValue = value;
                            //Debug.Log(possibleMoves[i].startCoord.rankIndex + " " + possibleMoves[i].startCoord.fileIndex);
                            //Debug.Log(bestValue + "i");
                        }
                        //if (value > alpha)
                        //    alpha = value;
                        //if (alpha <= beta)
                        //    break;
                    }
                  
                   // transpositionTable.Add(state.ToString(), bestValue);   
                    return bestValue;
                }

                else{
                    //Debug.Log(state.turn + "bb");

                    double bestValue = 100;
                    List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);

                    for(int i=0; i<possibleMoves.Count; i++){

                        ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                        double value = MiniMax(successorState, alpha, beta, depth-1, 0);
                        if(value < bestValue)
                            bestValue = value;
                        //Debug.Log(possibleMoves[i].startCoord.rankIndex + " " + possibleMoves[i].startCoord.fileIndex);
                        //Debug.Log(bestValue + "j");
                        //if (value < beta)
                        //    beta = value;
                        //if (beta <= alpha)
                        //    break;
                    }

                    //transpositionTable.Add(state.ToString(), bestValue);
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

            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);

            numberOfTraversedNodes = 0;
            transpositionTable.Clear();

            int randomNumber = GenerateRandomNumber(possibleMoves.Count);
            Move bestMove = possibleMoves[randomNumber];
            //ChessState tempState = gameLogic.GenerateMoveSuccessor(state, bestMove);
            


            //double bestPointControl = EvaluateBoard(tempState);
            double bestPointWhite = -999;
            double bestPointBlack = 999;

            double bestValue = 0;
            double minAlpha = -99;
            double maxBeta = 99;
            double tempValue;

            float oldTimeSeconds = Time.realtimeSinceStartup;

            for(int i = 0; i<possibleMoves.Count; i++){
                if(state.turn == 0){
                    ChessState tempState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                    
                    tempValue = MiniMax(tempState, minAlpha, maxBeta, 2, 1-state.turn);

                    if(tempValue >= bestPointWhite){ 
                        bestPointWhite = tempValue;
                        bestMove = possibleMoves[i];
                        //Debug.Log(possibleMoves[i].startCoord.rankIndex + " " + possibleMoves[i].startCoord.fileIndex);
                        //Debug.Log(bestPoint + "k");
                    }
                }
                if(state.turn == 1){
                    ChessState tempState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                    tempValue = MiniMax(tempState, minAlpha, maxBeta, 2, 1-state.turn);
                    if(tempValue < bestPointBlack){
                        bestPointBlack = tempValue;
                        bestMove = possibleMoves[i];
                        //Debug.Log(possibleMoves[i].startCoord.rankIndex + " " + possibleMoves[i].startCoord.fileIndex);
                        //Debug.Log(bestPoint + "l");
                    }
                }
            }

            /*
            if(state.turn == 0 && bestPoint <= bestPointControl){
                randomNumber = GenerateRandomNumber(possibleMoves.Count);
                bestMove = possibleMoves[randomNumber];
            }
            if(state.turn == 1 && bestPoint >= bestPointControl){
                randomNumber = GenerateRandomNumber(possibleMoves.Count);
                bestMove = possibleMoves[randomNumber];
            }
            */
            Debug.Log("Traversed nodes: " + numberOfTraversedNodes);
            float newTimeSeconds = Time.realtimeSinceStartup;
            Debug.Log("ElapsedTime: " + (newTimeSeconds - oldTimeSeconds));


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
        private double GetIfCenter(ChessState state){
            double score = 0;

            string pieceNotationC11 = state.board[3,3];
            if(pieceNotationC11 == "WP") score += 2;          
            if(pieceNotationC11 == "WR") score += 1; 
            if(pieceNotationC11 == "WN") score += 4;           
            if(pieceNotationC11 == "WB") score += 1;                   
            if(pieceNotationC11 == "WQ") score += 3;
            if(pieceNotationC11 == "WK") score -= 2;
                        
            if(pieceNotationC11 == "BP") score -= 2;
            if(pieceNotationC11 == "BR") score -= 1;
            if(pieceNotationC11 == "BN") score -= 4;
            if(pieceNotationC11 == "BB") score -= 1;
            if(pieceNotationC11 == "BQ") score -= 3;
            if(pieceNotationC11 == "BK") score += 2;


            string pieceNotationC12 = state.board[3,4];
            if(pieceNotationC12 == "WP") score += 2;          
            if(pieceNotationC12 == "WR") score += 1; 
            if(pieceNotationC12 == "WN") score += 4;           
            if(pieceNotationC12 == "WB") score += 1;                   
            if(pieceNotationC12 == "WQ") score += 3;
            if(pieceNotationC12 == "WK") score -= 2;
                        
            if(pieceNotationC12 == "BP") score -= 2;
            if(pieceNotationC12 == "BR") score -= 1;
            if(pieceNotationC12 == "BN") score -= 4;
            if(pieceNotationC12 == "BB") score -= 1;
            if(pieceNotationC12 == "BQ") score -= 3;
            if(pieceNotationC12 == "BK") score += 2;
    

            string pieceNotationC21 = state.board[4,3];
            if(pieceNotationC21 == "WP") score += 2;          
            if(pieceNotationC21 == "WR") score += 1; 
            if(pieceNotationC21 == "WN") score += 4;           
            if(pieceNotationC21 == "WB") score += 1;                   
            if(pieceNotationC21 == "WQ") score += 3;
            if(pieceNotationC21 == "WK") score -= 2;
                        
            if(pieceNotationC21 == "BP") score -= 2;
            if(pieceNotationC21 == "BR") score -= 1;
            if(pieceNotationC21 == "BN") score -= 4;
            if(pieceNotationC21 == "BB") score -= 1;
            if(pieceNotationC21 == "BQ") score -= 3;
            if(pieceNotationC21 == "BK") score += 2;
            

            string pieceNotationC22 = state.board[4,4];
            if(pieceNotationC22 == "WP") score += 2;          
            if(pieceNotationC22 == "WR") score += 1; 
            if(pieceNotationC22 == "WN") score += 4;           
            if(pieceNotationC22 == "WB") score += 1;                   
            if(pieceNotationC22 == "WQ") score += 3;
            if(pieceNotationC22 == "WK") score -= 2;
                        
            if(pieceNotationC22 == "BP") score -= 2;
            if(pieceNotationC22 == "BR") score -= 1;
            if(pieceNotationC22 == "BN") score -= 2;
            if(pieceNotationC22 == "BB") score -= 2;
            if(pieceNotationC22 == "BQ") score -= 3;
            if(pieceNotationC22 == "BK") score += 2;

            return score;
        }

        private double EvaluateCheckForMove(ChessState state){
            int turn = state.turn;
            double currentBest = 0;
            List<Move> moves = gameLogic.GenerateBoardMoves(state);
                double k = -999,l = -999;
                /*for (int i = 0; i < state.board.GetLength(0); i++){
                    for (int j = 0; j < state.board.GetLength(1); j++){
                        if(state.board[i,j] == "BK" && turn == 1){
                            k = i;
                            l = j;
                        }
                        if(state.board[i,j] == "WK" && turn == 0){
                            k = i;
                            l = j;
                        }
                    }
                }
                */
                if( turn == 0){
                    k = state.Search("BK").rankIndex;
                    l = state.Search("BK").fileIndex;
                }                   
                if(turn == 1){
                    k = state.Search("WK").rankIndex;
                    l = state.Search("WK").fileIndex;
                }
               

                for(int f = 0; f<moves.Count; f++){
                int m = moves[f].targetCoord.rankIndex;
                int n = moves[f].targetCoord.fileIndex;
                if((!state.IsEmpty(moves[f].targetCoord)) && moves[f].targetCoord.rankIndex == k && moves[f].targetCoord.fileIndex == l){
                    currentBest +=4;
                }    
            }
            return currentBest;
        }
   
        private double EvaluatePossibleCheckForMove(ChessState state){
                double currentBest = 0;
                List<Move> moves = gameLogic.GenerateBoardMoves(state);
                List<Move> movesOpponent = gameLogic.GenerateBoardMovesForOpponent(state);
                List<Move> movesOpponentKing = new List<Move>();
                for(int p = 0; p < movesOpponent.Count; p++){
                    if(state.GetPieceNotation(movesOpponent[p].startCoord) == 'K'){
                        movesOpponentKing.Add(movesOpponent[p]);
                    }
                }
                for(int f = 0; f<moves.Count; f++){
                    double possibleKingCheck = 0;
                    for(int z = 0; z < movesOpponentKing.Count; z++){
                            if(moves[f].targetCoord == movesOpponentKing[z].targetCoord){   
                                    possibleKingCheck += 0.1;
                            }              
                    }
                currentBest += possibleKingCheck;
                }
                return currentBest;
            }
   
   
    }


}