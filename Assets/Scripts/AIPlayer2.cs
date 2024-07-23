
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;


using System;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using System.Security.Cryptography;

namespace SoftwareKingdom.Chess.UI
{

    [CreateAssetMenu(fileName = "AIPlayer2", menuName = "SoftwareKingdom/Chess/AIPlayer2", order = 2)]

    public class AIPlayer2 : ChessPlayer {

        // Settings

        // Connections
        public ChessLogic gameLogic;
        ChessState currentChessState;

        // State variables
        //Dictionary <string, double> transpositionTable = new Dictionary<string, double>();
        int numberOfTraversedNodes = 0;
        int hamle = 0;
    
        public override bool IsHuman() { return false; }

        public override void Configure(ChessLogic gameLogic){
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {

            if(gameLogic.gameEnd != true){
                gameLogic.PlayMove(state, GetBestMoveOfAll(state));
                hamle++;
                Debug.Log(hamle);
            }
            else 
                hamle = 0;
           
        }

       
        
        private double EvaluateBoard(ChessState state){
            double point = 0;
            point += GetBoardPieceStatus(state);
            //point += GetIfCenter(state);
            //point += EvaluateCheckForMove(state);
            //point += EvaluatePossibleCheckForMove(state);
            return point;
        }

        
        /*
        // Minimax with ToString Transposition Tables
        private double MiniMax(ChessState state, double alpha, double beta, int depth, int maximizingPlayer) {

            // Check transposition table
            double savedValue;
            numberOfTraversedNodes++;

            if (transpositionTable.TryGetValue(state.ToString(), out savedValue))
            {
                // Debug.Log("Transposition Table works");
                return savedValue;
            }

            if (depth == 0)
            {
                double value = EvaluateBoard(state);
                transpositionTable[state.ToString()] = value;
                return value;
            }

            if (maximizingPlayer == 0)
            {
                double bestValue = double.NegativeInfinity;
                List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
                //possibleMoves = OrderMoves(possibleMoves, state); // Move ordering uygulanıyor


                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                    double value = MiniMax(successorState, alpha, beta, depth - 1, 1);
                    bestValue = Math.Max(bestValue, value);
                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                        break; // Beta cutoff
                }

                transpositionTable[state.ToString()] = bestValue;
                return bestValue;
            }
            else
            {
                double bestValue = double.PositiveInfinity;
                List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
                //possibleMoves = OrderMoves(possibleMoves, state); // Move ordering uygulanıyor



                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    ChessState successorState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                    double value = MiniMax(successorState, alpha, beta, depth - 1, 0);
                    bestValue = Math.Min(bestValue, value);
                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                        break; // Alpha cutoff
                }

                transpositionTable[state.ToString()] = bestValue;
                return bestValue;
            }
        }
        
        */

        
        private Dictionary<ulong, double> transpositionTable = new Dictionary<ulong, double>();
        private ZobristHashing zobristHashing = new ZobristHashing();


        private double MiniMax(ChessState state, double alpha, double beta, int depth, int maximizingPlayer)
        {
            numberOfTraversedNodes++;

            ulong hash = zobristHashing.ComputeHash(state);
            
            double savedValue;

            if (transpositionTable.TryGetValue(hash, out savedValue))            
            {
                    return savedValue;
            }
            

            if (depth == 0)
            {
                double value = EvaluateBoard(state);
                transpositionTable[hash] = (value);
                return value;
            }

            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            //possibleMoves = OrderMoves(state, possibleMoves); // Move ordering uygulanıyor

            if (maximizingPlayer == 0)
            {
                double bestValue = double.NegativeInfinity;
                foreach (var move in possibleMoves)
                {
                    ChessState successorState = gameLogic.GenerateMoveSuccessor(state, move);

                    double value = MiniMax(successorState, alpha, beta, depth - 1, 1);
                    bestValue = Math.Max(bestValue, value);
                    alpha = Math.Max(alpha, bestValue);

                    if (beta <= alpha)
                        break; // Beta cutoff
                }
                transpositionTable[hash] = (bestValue);                
                return bestValue;
            }
            else
            {
                double bestValue = double.PositiveInfinity;
                foreach (var move in possibleMoves)
                {
                    ChessState successorState = gameLogic.GenerateMoveSuccessor(state, move);

                    double value = MiniMax(successorState, alpha, beta, depth - 1, 0);
                    
                    bestValue = Math.Min(bestValue, value);
                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                        break; // Alpha cutoff
                }


                transpositionTable[hash] = (bestValue);                
                return bestValue;
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

            numberOfTraversedNodes = 0;
            transpositionTable.Clear();

            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);

            

            int randomNumber = GenerateRandomNumber(possibleMoves.Count);
                
            Move bestMove = possibleMoves[randomNumber];
            
            


            //double bestPointControl = EvaluateBoard(tempState);
            double bestPointWhite = -999;
            double bestPointBlack = 999;

            double bestValue = 0;
            double minAlpha = double.NegativeInfinity;
            double maxBeta = double.PositiveInfinity;
            double tempValue;

            float oldTimeSeconds = Time.realtimeSinceStartup;


            for(int i = 0; i<possibleMoves.Count; i++){
                if(state.turn == 0){
                    ChessState tempState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                    
                    tempValue = MiniMax(tempState, minAlpha, maxBeta, 2, 1 - state.turn);
                    if(tempValue >= bestPointWhite){ 
                        bestPointWhite = tempValue;
                        bestMove = possibleMoves[i];
                        //Debug.Log(possibleMoves[i].startCoord.rankIndex + " " + possibleMoves[i].startCoord.fileIndex);
                        //Debug.Log(bestPoint + "k");
                    }
                }
                if(state.turn == 1){
                    ChessState tempState = gameLogic.GenerateMoveSuccessor(state, possibleMoves[i]);
                    tempValue = MiniMax(tempState, minAlpha, maxBeta, 2, 1 - state.turn);
                    if(tempValue <= bestPointBlack){
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
                        if(pieceNotation == "WK") score += 100;
                        
                        if(pieceNotation == "BP") score -= 2;
                        if(pieceNotation == "BR") score -= 5;
                        if(pieceNotation == "BN") score -= 3;
                        if(pieceNotation == "BB") score -= 3;
                        if(pieceNotation == "BQ") score -= 9;
                        if(pieceNotation == "BK") score -= 100;
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

          
        
        public class ZobristHashing
        {
            private const int boardSize = 65; // 8x8 satranç tahtası
            private const int pieceTypes = 12; // 6 taş türü (her iki renk için)
            private ulong[,] zobristTable;
            private Dictionary<string, int> pieceMapping;

            public ZobristHashing()
            {
                zobristTable = new ulong[boardSize, pieceTypes];
                pieceMapping = new Dictionary<string, int>
                {
                    { "WR", 0 }, { "WN", 1 }, { "WB", 2 }, { "WQ", 3 }, { "WK", 4 }, { "WP", 5 },
                    { "BR", 6 }, { "BN", 7 }, { "BB", 8 }, { "BQ", 9 }, { "BK", 10 }, { "BP", 11 }
                };
                InitZobristTable();
            }

            /*
            private void InitZobristTable()
            {
                System.Random rng = new System.Random();
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < pieceTypes; j++)
                    {
                        zobristTable[i, j] = (ulong)(rng.NextDouble() * ulong.MaxValue);
                    }
                }
            }
            */
            private void InitZobristTable()
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] buffer = new byte[8]; // 8 bytes = 64 bits
                    for (int i = 0; i < boardSize; i++)
                    {
                        for (int j = 0; j < pieceTypes; j++)
                        {
                            rng.GetBytes(buffer);
                            zobristTable[i, j] = BitConverter.ToUInt64(buffer, 0);
                        }
                    }
                }
            }

            public ulong ComputeHash(ChessState board)
            {
                ulong hash = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        string piece = board[i, j];
                        if (piece != null)
                        {
                            int pieceIndex = pieceMapping[piece];
                            int squareIndex = i * 8 + j;
                            hash ^= zobristTable[squareIndex, pieceIndex];
                        }
                    }
                }
                hash ^= zobristTable[64, board.turn];
                return hash;
            }

            public ulong UpdateHash(ulong currentHash, string piece, Coord from, Coord to)
            {
                int pieceIndex = pieceMapping[piece];
                int fromSquareIndex = from.rankIndex * 8 + from.fileIndex;
                int toSquareIndex = to.rankIndex * 8 + to.fileIndex;

                currentHash ^= zobristTable[fromSquareIndex, pieceIndex];
                currentHash ^= zobristTable[toSquareIndex, pieceIndex];
                return currentHash;
            }
        }
        
        
        

        private List<Move> OrderMoves(ChessState state, List<Move> moves)
        {
            // Simple heuristic: captures first, then checks, then others
            moves.Sort((move1, move2) =>
            {
                int score1 = EvaluateMove(state, move1);
                int score2 = EvaluateMove(state, move2);
                return score2.CompareTo(score1); // Descending order
            });
            return moves;
        }

        private int EvaluateMove(ChessState state, Move move)
        {
            // Simple heuristic: value of the captured piece
            string targetPiece = state.GetPieceAt(move.targetCoord.rankIndex, move.targetCoord.fileIndex);
            return targetPiece != null ? GetPieceValue(targetPiece) : 0;
        }

        private int GetPieceValue(string piece)
        {
            switch (piece)
            {
                case "WP": return 2;
                case "WR": return 5;
                case "WN": return 3;
                case "WB": return 3;
                case "WQ": return 9;
                case "WK": return 100;
                case "BP": return -2;
                case "BR": return -5;
                case "BN": return -3;
                case "BB": return -3;
                case "BQ": return -9;
                case "BK": return -100;
                default: return 0;
            }
        }
        
        private List<Move> OrderMoves1(List<Move> moves, ChessState state)
        {
            // Capture moves first, then check moves, then others
            List<Move> captureMoves = new List<Move>();
            List<Move> checkMoves = new List<Move>();
            List<Move> otherMoves = new List<Move>();

            foreach (var move in moves)
            {

                if (state.GetPieceNotation(move.targetCoord) != null && state.GetColor(state.GetPieceAt(move.targetCoord.rankIndex, move.targetCoord.fileIndex)) == 1)
                {
                    captureMoves.Add(move);
                }
                else if (gameLogic.IsCheck(state, move))
                {
                    checkMoves.Add(move);
                }
                else
                {
                    otherMoves.Add(move);
                }
            }

            // Combine all moves
            //captureMoves.AddRange(checkMoves);
            captureMoves.AddRange(otherMoves);
            return captureMoves;
        }
        
        
        /*
        private List<Move> OrderMoves(List<Move> moves, ChessState state)
        {
            return moves.OrderByDescending(move => GetMoveScore(move, state)).ToList();
        }

        private int GetMoveScore(Move move, ChessState state)
        {
            int score = 0;

            // Generate the successor state
            ChessState newState = gameLogic.GenerateMoveSuccessor(state, move);

            // Prioritize capture moves
            if (newState.GetPieceNotation(move.targetCoord) != BaseChessLogic.EMPTY_NOTATION)
            {
                score += 100 + GetPieceValue(newState.GetPieceNotation(move.targetCoord));
            }

            // Prioritize check moves
            if (gameLogic.IsCheck(newState, move))
            {
                score += 50;
            }

            // Additional heuristics can be added here

            return score;
        }

        private int GetPieceValue(char piece)
        {
            switch (piece)
            {
                case BaseChessLogic.PAWN_PIECE_NOTATION: return 1;
                case BaseChessLogic.KNIGHT_PIECE_NOTATION: return 3;
                case BaseChessLogic.BISHOP_PIECE_NOTATION: return 3;
                case BaseChessLogic.ROOK_PIECE_NOTATION: return 5;
                case BaseChessLogic.QUEEN_PIECE_NOTATION: return 9;
                case BaseChessLogic.KING_PIECE_NOTATION: return 1000; // High value for the king
                default: return 0;
            }
        }
        */
       
   
   
    }


}