
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
            
            double bestMoveWhite = -300;
            double bestMoveBlack = 300;

            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            Move bestMove = possibleMoves[0];
            if(possibleMoves.Count != 0){
            bestMove = possibleMoves[GenerateRandomNumber(possibleMoves.Count)];
            }

            //Move move = possibleMoves[0];
            for(int k =0; k < possibleMoves.Count; k++){

                ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[k]);
                double currentBest = 0;
                currentBest = GetBoardStatus(successorState);

                //currentBest = EvaluateCheckForMove(successorState, possibleMoves[k], currentBest);
                //currentBest = EvaluatePossibleCheckForMove(successorState, state, currentBest);
                //currentBest = IsItSameMove(possibleMoves[k], successorState, currentBest);
                //currentBest = IsItSamePiece(possibleMoves[k], successorState, currentBest);
                //currentBest = CheckTheSquare(successorState, possibleMoves[k], currentBest);
                

                

                if(state.turn == 0){
                    /*
                    if ( bestMoveWhite == currentBest){
                        //currentBest = EvaluateCheckForMove(successorState, possibleMoves[k], currentBest);
                        //currentBest = EvaluatePossibleCheckForMove(successorState, state, currentBest);
                        //currentBest = IsItSameMove(possibleMoves[k], successorState, currentBest);
                        //currentBest = IsItSamePiece(possibleMoves[k], successorState, currentBest);
                        //currentBest = CheckTheSquare(successorState, possibleMoves[k], currentBest);

                        int randomNumberWHite = GenerateRandomNumber(possibleMoves.Count);
                        bestMove = possibleMoves[randomNumberWHite];
                    }
                    */

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
                    /*
                    if( bestMoveBlack == currentBest){
                        //currentBest = EvaluateCheckForMove(successorState, possibleMoves[k], currentBest);
                        //currentBest = EvaluatePossibleCheckForMove(successorState, state, currentBest);
                        //currentBest = IsItSameMove(possibleMoves[k], successorState, currentBest);
                        //currentBest = IsItSamePiece(possibleMoves[k], successorState, currentBest);
                        //currentBest = CheckTheSquare(successorState, possibleMoves[k], currentBest);

                        int randomNumberBlack = GenerateRandomNumber(possibleMoves.Count);
                        bestMove = possibleMoves[randomNumberBlack];
                        

                    }
                    */
                  
                }
            }

            if(possibleMoves.Count != 0){
                //int checkLastPiece = CheckLastPiece(state);
                //Debug.Log(checkLastPiece);

                
                if(state.turn == 0 && bestMoveWhite == GetBoardStatus(state)){
                        
                    int randomNumberWHite = GenerateRandomNumber(possibleMoves.Count);
                    bestMove = possibleMoves[randomNumberWHite];
                    

                    }
                if(state.turn == 1 && bestMoveBlack == GetBoardStatus(state)){
                        
                    int randomNumberBlack = GenerateRandomNumber(possibleMoves.Count);
                    bestMove = possibleMoves[randomNumberBlack];
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

        private double CheckLastPiece(ChessState state){
                double checkLastPieceWhite = 0;
                double checkLastPieceBlack = 0;

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

        private double GetBoardStatus(ChessState boardState){
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
        /*
        private int EvaluateCheckForMove(ChessState state, int currentBest){
            List<Move> moves = gameLogic.GenerateBoardMoves(state);
            for(int f = 0; f<moves.Count; f++){
                ChessState successorState = gameLogic.GenerateMoveSuccessor(state, moves[f]);

                for (int i = 0; i < successorState.board.GetLength(0); i++){
                    for (int j = 0; j < successorState.board.GetLength(1); j++){

                        if(successorState.IsEmpty(i, j) || !state.IsInTurn(successorState[i, j])) continue;
                        if(state.IsInTurn(successorState[i, j])){
                            if(successorState[moves[f].targetCoord.rankIndex, moves[f].targetCoord.fileIndex] == "BK"){
                                currentBest += 4;
                                Debug.Log("+4");
                            }
                            if(successorState.GetPieceNotation(moves[f].targetCoord) == 'K' && successorState.GetColor(successorState[i, j]) == 1){
                                currentBest -= 4;
                            }
                        }
                    }
                }

            }
            return currentBest;
        }
        */
        private double EvaluateCheckForMove(ChessState state, Move playedMove, double currentBest){
            List<Move> moves = gameLogic.GenerateBoardMovesForOpponent(state);
                double k = -999,l = -999;
                for (int i = 0; i < state.board.GetLength(0); i++){
                    for (int j = 0; j < state.board.GetLength(1); j++){
                        if(state.board[i,j] == "BK" && state.turn == 1){
                            k = i;
                            l = j;
                        }
                        if(state.board[i,j] == "WK" && state.turn == 0){
                            k = i;
                           l = j;
                        }

                    }
                } 
            for(int f = 0; f<moves.Count; f++){
                int m = moves[f].targetCoord.rankIndex;
                int n = moves[f].targetCoord.fileIndex;

                //for (int i = 0; i < state.board.GetLength(0); i++){
                  //  for (int j = 0; j < state.board.GetLength(1); j++){

                        //Debug.Log(state.board[i,j] + "1");

                        //if(state.IsEmpty(i, j) || !state.IsInTurn(state[i, j])) continue;

                        //if(state.IsInTurn(state[i, j])){

                            //Debug.Log(state.board[i,j] + "2");
                            //Debug.Log(state.board[k,l] + "6");


                            
                            //Debug.Log(m);
                            //Debug.Log(n);
                            //Debug.Log(k);
                            //Debug.Log(l);
                            //Debug.Log("işl22");

                            if(!(state.IsEmpty(moves[f].targetCoord)) && moves[f].targetCoord.rankIndex == k && moves[f].targetCoord.fileIndex == l){
                                currentBest +=2.5;
                            }
                            //List<Move> movesOwn = gameLogic.GenerateBoardMoves(state);
                            //for(int i=0; i<movesOwn.Count; i++){
                            //    if(movesOwn[i].targetCoord == playedMove.targetCoord && moves[f].targetCoord.rankIndex == k && moves[f].targetCoord.fileIndex == l){
                            //    Debug.Log("kkk");
                            //    currentBest +=3;
                            //    }
                            //}
                            /*if(state.GetPieceNotation(moves[f].targetCoord) == 'K' && state.GetColor(state[i, j]) == 1){
                                currentBest -= 4;
                            }
                            */
                        }
                    //}
                //}

            //}
            return currentBest;
        }

        private double EvaluatePossibleCheckForMove(ChessState state, ChessState oldState, double currentBest){
            List<Move> moves = gameLogic.GenerateBoardMovesForOpponent(state);
            List<Move> movesOpponent = gameLogic.GenerateBoardMoves(state);
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
                            
                                possibleKingCheck += 0.8;


                        }
                    
                }
            currentBest += possibleKingCheck;

            }
            return currentBest;
        }

        private double IsItSameMove(Move playedMove, ChessState successorState, double currentBest){
            List<Move> moves = gameLogic.GenerateBoardMovesForOpponent(successorState);
            for(int i=0; i<moves.Count; i++){
                if(playedMove.startCoord == moves[i].targetCoord || successorState.GetPieceNotation(playedMove.startCoord) ==  successorState.GetPieceNotation(moves[i].targetCoord)){
                    Debug.Log("ğğğ");
                    currentBest -=2;
                }
            }
            return currentBest;

        }

        private double IsItSamePiece(Move playedMove, ChessState successorState, double currentBest){
            List<Move> moves = gameLogic.GenerateBoardMovesForOpponent(successorState);
            for(int i=0; i<moves.Count; i++){
                if(successorState.GetPieceNotation(playedMove.startCoord) == successorState.GetPieceNotation(moves[i].startCoord)){
                    Debug.Log("ddddd");
                    currentBest -=1;
                }
            }
            return currentBest;

        }

        private double CheckTheSquare(ChessState successorState, Move playedMove, double currentBest){
            List<Move> moves = gameLogic.GenerateBoardMoves(successorState);
            for(int i=0; i<moves.Count; i++){
                if(moves[i].targetCoord == playedMove.targetCoord){
                    Debug.Log("kkk");
                    currentBest +=3;
                }
            }
            return currentBest;

        }

    }
}