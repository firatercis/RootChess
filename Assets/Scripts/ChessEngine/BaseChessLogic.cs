
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace SoftwareKingdom.Chess.Core
{

    [CreateAssetMenu(fileName = "StandardChessLogic", menuName = "SoftwareKingdom/Chess/StandardChessLogic", order = 1)]
    public class BaseChessLogic : ChessLogic 
    {
        const string VARIANT_NAME = "Standard";

        const char BLACK_PIECE_NOTATION = 'B';
        const char WHITE_PIECE_NOTATION = 'W';
        const char BISHOP_PIECE_NOTATION = 'B';
        const char QUEEN_PIECE_NOTATION = 'Q';
        const char ROOK_PIECE_NOTATION = 'R';
        const char KING_PIECE_NOTATION = 'K';
        const char KNIGHT_PIECE_NOTATION = 'N';
        const char PAWN_PIECE_NOTATION = 'P';


        public const char EMPTY_NOTATION = '\0';

        const int N_DIAGONAL_DIRECTIONS = 4;
        const int N_STRAIGHT_DIRECTIONS = 4;

        public static string[,] standardChessInitialBoard =
        {
            { "WR", "WN","WB", "WQ", "WK", "WB","WN","WR" },
            { "WP", "WP","WP","WP","WP","WP","WP" ,"WP" },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { "BP", "BP","BP","BP","BP","BP","BP" ,"BP" },
            { "BR", "BN","BB", "BQ", "BK", "BB","BN","BR" },
        };

        public static char[] pieceLetters = { 'P', 'N', 'B', 'R', 'Q', 'K' };

        static string[] initialFlags = { "WC0-0", "WC0-0-0", "BC0-0", "BC0-0-0" };


        // Settings

        // Connections
        // ChessGame chessGame;

        Dictionary<char, IPieceMoveGenerator> pieceMoveGeneratorsDict;

        public BaseChessLogic()
        {

            pieceMoveGeneratorsDict = CreateStandardPieces();

           // variantName = "Standard";
        }

        public override ChessState CreateGame() {



            currentState = CreateStandardInitialState();
            return currentState;
        }

        public static ChessState CreateStandardInitialState()
        {

            ChessState initialBoardState = new ChessState(standardChessInitialBoard, new List<string>(initialFlags));
            return initialBoardState;
        }



        public static  Dictionary<char, IPieceMoveGenerator> CreateStandardPieces()
        {
            // Pawn
            PawnMovesGenerator pawnMoveGenerator = new PawnMovesGenerator();
            // Knight
            KnightMovesGenerator knightMoveGenerator = new KnightMovesGenerator();
            // K
            Coord[] diagonalDirections = new Coord[N_DIAGONAL_DIRECTIONS];
            diagonalDirections[0] = new Coord(-1, 1);
            diagonalDirections[1] = new Coord(1, -1);
            diagonalDirections[2] = new Coord(1, 1);
            diagonalDirections[3] = new Coord(-1, -1);
            SlidingPieceMovesGenerator bishopMoveGenerator = new SlidingPieceMovesGenerator(diagonalDirections, BISHOP_PIECE_NOTATION);

            // Rook
            Coord[] straightDirections = new Coord[N_STRAIGHT_DIRECTIONS];
            straightDirections[0] = new Coord(-1, 0);
            straightDirections[1] = new Coord(1, 0);
            straightDirections[2] = new Coord(0, -1);
            straightDirections[3] = new Coord(0, 1);
            SlidingPieceMovesGenerator rookMoveGenerator = new SlidingPieceMovesGenerator(straightDirections, ROOK_PIECE_NOTATION);

            // Queen
            Coord[] allDirections = new Coord[N_DIAGONAL_DIRECTIONS + N_STRAIGHT_DIRECTIONS];
            diagonalDirections.CopyTo(allDirections, 0);
            straightDirections.CopyTo(allDirections, diagonalDirections.Length);
            SlidingPieceMovesGenerator queenMoveGenerator = new SlidingPieceMovesGenerator(allDirections, QUEEN_PIECE_NOTATION);
            // King
            KingMoveGenerator kingMoveGenerator = new KingMoveGenerator();


            IPieceMoveGenerator[] pieceMoveGenerators = new IPieceMoveGenerator[] { pawnMoveGenerator, knightMoveGenerator, bishopMoveGenerator, rookMoveGenerator, queenMoveGenerator, kingMoveGenerator };

            Dictionary<char, IPieceMoveGenerator> pieceMoveGeneratorsDict = new Dictionary<char, IPieceMoveGenerator>();

            for(int i=0; i<pieceLetters.Length; i++)
            {
                pieceMoveGeneratorsDict.Add(pieceLetters[i], pieceMoveGenerators[i]);
            }

            return pieceMoveGeneratorsDict;

        }

        public override List<Move> GenerateMoves(ChessState boardState, Coord sourceCoord, bool checkLegal = true) {
            List<Move> result = new List<Move>();

            char currentPiece = boardState.GetPieceNotation(sourceCoord);


            if(currentPiece != EMPTY_NOTATION && boardState.IsInTurn(boardState[sourceCoord]))
            {
                Move[] possibleMovesForPiece = pieceMoveGeneratorsDict[currentPiece].GenerateMoves(boardState, sourceCoord);
                result.AddRange(possibleMovesForPiece);
            }

            // Filter non-legal moves if check legal is selected.

            

            result = checkLegal ? result.Where(x => IsLegal(x, boardState)).ToList() : result; 
        
            return result;

        }

        // This function generates all possible moves for the opponent on the current board
        public List<Move> GenerateMovesForOpponent(ChessState boardState, Coord sourceCoord, bool checkLegal = true) {
            List<Move> result = new List<Move>();

            char currentPiece = boardState.GetPieceNotation(sourceCoord);


            if(currentPiece != EMPTY_NOTATION && !boardState.IsInTurn(boardState[sourceCoord]))
            {
                Move[] possibleMovesForPiece = pieceMoveGeneratorsDict[currentPiece].GenerateMoves(boardState, sourceCoord);
                result.AddRange(possibleMovesForPiece);
            }

            // Filter non-legal moves if check legal is selected.

            result = checkLegal ? result.Where(x => IsLegal(x, boardState)).ToList() : result; 
        
            return result;
        }


        public virtual bool IsLegal(Move move, ChessState boardState) {
            ChessState nextBoardState = GenerateMoveSuccessor(boardState, move);
            List<Move> opponentMoves = GenerateBoardMoves(nextBoardState, checkLegal: false);


            bool result = true;
            for (int i = 0; i < opponentMoves.Count; i++)
            {
                if(move.targetCoord == new Coord(0,6) && move.startCoord == new Coord(0,4) && boardState.GetPieceNotation(move.startCoord) == KING_PIECE_NOTATION){
                if(opponentMoves[i].targetCoord == new Coord(0,4) || opponentMoves[i].targetCoord == new Coord(0,5) || opponentMoves[i].targetCoord == new Coord(0,6))
                        result = false;
                }
                if(move.targetCoord == new Coord(0,2) && move.startCoord == new Coord(0,4) && boardState.GetPieceNotation(move.startCoord) == KING_PIECE_NOTATION){
                    if(opponentMoves[i].targetCoord == new Coord(0,4) || opponentMoves[i].targetCoord == new Coord(0,3) || opponentMoves[i].targetCoord == new Coord(0,2))
                        result = false;
                }
                if(move.targetCoord == new Coord(7,6) && move.startCoord == new Coord(7,4) && boardState.GetPieceNotation(move.startCoord) == KING_PIECE_NOTATION){
                    if(opponentMoves[i].targetCoord == new Coord(7,4) || opponentMoves[i].targetCoord == new Coord(7,5) || opponentMoves[i].targetCoord == new Coord(7,6))
                        result = false;
                }
                if(move.targetCoord == new Coord(7,2) && move.startCoord == new Coord(7,4) && boardState.GetPieceNotation(move.startCoord) == KING_PIECE_NOTATION){
                    if(opponentMoves[i].targetCoord == new Coord(7,4) || opponentMoves[i].targetCoord == new Coord(7,3) || opponentMoves[i].targetCoord == new Coord(7,2))
                        result = false;
                }

                if (nextBoardState.IsEmpty(opponentMoves[i].targetCoord)) continue;

                if (nextBoardState.GetPieceNotation(opponentMoves[i].targetCoord) == KING_PIECE_NOTATION) // Check if some enemy piece can take a king
                {
                    result = false;
                    break;
                }
               
               
                
                
            }
            return result;
        }

     

        public override ChessState GenerateMoveSuccessor(ChessState inputState, Move move) {
            ChessState outputState = new ChessState(inputState);
            ApplyMove( outputState, move);
            return outputState;
        }

        public override void ApplyMove( ChessState boardState, Move move) {

            boardState[move.targetCoord] = boardState[move.startCoord];
            boardState[move.startCoord] = ChessState.EMPTY_SQUARE;

            // Change en passant state
            SaveEnPassantCoord(boardState, move);
            // Apply en passant move
            CheckApplyEnPassant(boardState, move);

            // Change Castling state
            CheckUpdateCastling(boardState, move);
            // Apply castling move
            ApplyCastling(boardState, move);

            // Apply the promotion
            CheckApplyPromotion(boardState, move);

            // CheckMate(boardState, move);
            
            SwitchTurn(boardState);

        }
      

        
        public override void CheckGameEnd(ChessState boardState){
            List<Move> moves = GenerateBoardMoves(boardState, true);
            List<Move> oppoenentMoves = GenerateBoardMovesForOpponent(boardState, true);

            //Debug.Log(moves.Count);
            //Debug.Log(oppoenentMoves.Count);
            //Debug.Log(boardState.GetTurnPrefix());

            bool isCheck = false;

            for(int i = 0; i<oppoenentMoves.Count; i++){
                if((boardState.GetPieceNotation(oppoenentMoves[i].targetCoord) == KING_PIECE_NOTATION)){
                    isCheck = true;
                }
            }

            if(isCheck && moves.Count == 0){
                //Debug.Log("Checkmate!, winner: " + (1-boardState.turn));
                int winningIndex = 1;
                if(boardState.turn == 0)
                {
                    winningIndex = -1;
                }
                OnGameEnd(new GameResult(winningIndex));
                gameEnd = true;
            }

            
            else if(!isCheck && moves.Count == 0){
                //Debug.Log("Tie! ");
                OnGameEnd(new GameResult(0));
                gameEnd = true;
            }
           
            int k = 0; 
            for(int i = 0; i < boardState.board.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.board.GetLength(1); j++)
                {
                    if (boardState.IsEmpty(i, j)) continue;
                   // Debug.Log(boardState.board[i,j]);
                    k++;
                }
            }
            if(k==2){
                //Debug.Log("tie");
                OnGameEnd(new GameResult(0));
                gameEnd = true;
                // Time.timeScale = 0;      
            }
            
        }

        /*
        private static void CheckMate(ChessState boardState, Move move){
            if(boardState.turn == 0){
                Coord kingCoord = new Coord(boardState.Search("K"));
                List<Move> possibleMoves = GenerateMoves(boardState, kingCoord);
                if(possibleMoves == null)
                    DebugDirectoryBuilder.Log("Black won");
            }
        }
        */

        // Enables En Passant move if the move is playebla
        private static void SaveEnPassantCoord(ChessState boardState, Move move) {

            // En Passant move is not valid
            if (move.specialCondition != SpecialConditions.PawnTwoForward)
            {
                boardState.enPassantCoord = ChessState.NO_EN_PASSANT_COORD;
            }

            // En Passant move is valid
            else
            {
                // Calculates the coordinate that the pawn will move if En Passant move is played
                int enPassantRankIndex = (move.targetCoord.rankIndex + move.startCoord.rankIndex) / 2; 
                int enPassantFileIndex = move.targetCoord.fileIndex;
                boardState.enPassantCoord = new Coord(enPassantRankIndex, enPassantFileIndex);
            }
        }

        // Applies En Passant move if it is played
        private static void CheckApplyEnPassant(ChessState boardState, Move move) {

            // Calculates the coordinates of the eaten pawn by En Passant move
            if (move.specialCondition == SpecialConditions.EnPassantCapture)
            {
                int oneBackward = -1;
                if (boardState.turn == ChessState.BLACK)
                {
                    oneBackward =1 ;
                }  

                boardState[move.targetCoord + new Coord(oneBackward, 0)] = ChessState.EMPTY_SQUARE; // Removes the eaten pawn by the En Passant move
            }
        }

        // Updates castling flags
        private void CheckUpdateCastling(ChessState boardState, Move move){
            
            // White player castle flags
            if(boardState.turn == ChessState.WHITE){
                // Removes white castle flags from the array when white king moves
                if(move.startCoord == new Coord(0,4)){
                    boardState.flags.Remove("WC0-0");
                    boardState.flags.Remove("WC0-0-0");
                }
                // Removes white king side castle flag from the array when white right rook moves
                if(move.startCoord == new Coord(0,7)){
                    boardState.flags.Remove("WC0-0");
                }
                // Removes white queen side castle flag from the array when white left rook moves
                if(move.startCoord == new Coord(0,0)){
                    boardState.flags.Remove("WC0-0-0");
                }
                
            } 
            // Black player castle flags
            if(boardState.turn == ChessState.BLACK){
                // Removes black castle flags from the array when black king moves
                if(move.startCoord == new Coord(7,4)){
                    boardState.flags.Remove("BC0-0");
                    boardState.flags.Remove("BC0-0-0");
                }
                // Removes black king side castle flag from the array when white black rook moves
                if(move.startCoord == new Coord(7,7)){
                    boardState.flags.Remove("BC0-0");
                }
                // Removes black queen side castle flag from the array when black left rook moves
                if(move.startCoord == new Coord(7,0)){
                    boardState.flags.Remove("BC0-0-0");
                }
            }
        }

        // Applies the castling move to the rooks (King is moved in the KingMoveGenerator class, GenerateCastlingMoves function)
        static void ApplyCastling(ChessState boardState, Move move){
            // Applies white player's castle moves
            if(boardState.turn == ChessState.WHITE){
                // Plays the white king side castle move
                if(move.specialCondition == SpecialConditions.Castling && move.targetCoord == new Coord(0,6)){
                    boardState[0,5] = boardState[0,7]; // Moves the rook
                    boardState[0,7] = ChessState.EMPTY_SQUARE; // Empties the rook's previous position
                }
                // Plays the white queen side castle move
                if(move.specialCondition == SpecialConditions.Castling && move.targetCoord == new Coord(0,2)){
                    boardState[0,3] = boardState[0,0];
                    boardState[0,0] = ChessState.EMPTY_SQUARE;
                }
            }
            // Applies black player's castle moves
            if(boardState.turn == ChessState.BLACK){
                // Plays the black king side castle move
                if(move.specialCondition == SpecialConditions.Castling && move.targetCoord == new Coord(7,6)){
                    boardState[7,5] = boardState[7,7];
                    boardState[7,7] = ChessState.EMPTY_SQUARE;
                }
                // Plays the black queen side castle move
                if(move.specialCondition == SpecialConditions.Castling && move.targetCoord == new Coord(7,2)){
                    boardState[7,3] = boardState[7,0];
                    boardState[7,0] = ChessState.EMPTY_SQUARE;
                }
            }
        }

        // Checks the promotion conditions 
        // Applies the promotion if conditions are met
        private static void CheckApplyPromotion(ChessState boardState, Move move){
            // Promotion of white pawns
            if(boardState.turn == ChessState.WHITE){
                // Promotion to Queen
                if(move.specialCondition == SpecialConditions.PromoteToQueen){
                    boardState[move.targetCoord] = "" + WHITE_PIECE_NOTATION + QUEEN_PIECE_NOTATION;
                }
                // Promotion to Bishop
                if(move.specialCondition == SpecialConditions.PromoteToBishop){
                    boardState[move.targetCoord] = "" + WHITE_PIECE_NOTATION + BISHOP_PIECE_NOTATION;
                }
                // Promotion to Knight
                if(move.specialCondition == SpecialConditions.PromoteToKnight){
                    boardState[move.targetCoord] = "" + WHITE_PIECE_NOTATION + KNIGHT_PIECE_NOTATION;
                }
                // Promotion to Rook
                if(move.specialCondition == SpecialConditions.PromoteToRook){
                    boardState[move.targetCoord] = "" + WHITE_PIECE_NOTATION + ROOK_PIECE_NOTATION;
                }
            }
            // Promotion of black pawns
            if(boardState.turn == ChessState.BLACK){
                // Promotion to Queen
                if(move.specialCondition == SpecialConditions.PromoteToQueen){
                    boardState[move.targetCoord] = "" + BLACK_PIECE_NOTATION + QUEEN_PIECE_NOTATION;
                }
                // Promotion to Bishop
                if(move.specialCondition == SpecialConditions.PromoteToBishop){
                    boardState[move.targetCoord] = "" + BLACK_PIECE_NOTATION + BISHOP_PIECE_NOTATION;
                }
                // Promotion to Knight
                if(move.specialCondition == SpecialConditions.PromoteToKnight){
                    boardState[move.targetCoord] = "" + BLACK_PIECE_NOTATION + KNIGHT_PIECE_NOTATION;
                }
                // Promotion to Rook
                if(move.specialCondition == SpecialConditions.PromoteToRook){
                    boardState[move.targetCoord] = "" + BLACK_PIECE_NOTATION + ROOK_PIECE_NOTATION;
                }
            }
        }

        
        public  override List<Move> GenerateBoardMoves(ChessState boardState) { // TODO: Can be virtual
            return GenerateBoardMoves(boardState, checkLegal: true);
        }
        public override List<Move> GenerateBoardMovesForOpponent(ChessState boardState) { // TODO: Can be virtual
            return GenerateBoardMovesForOpponent(boardState, checkLegal: false);
        }

       

        protected virtual List<Move> GenerateBoardMoves(ChessState boardState, bool checkLegal) {
            List<Move> pseudoLegalMoves = new List<Move>();
            for (int i = 0; i < boardState.board.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.board.GetLength(1); j++)
                {
                    if (boardState.IsEmpty(i, j) || !boardState.IsInTurn(boardState[i, j])) continue;

                    pseudoLegalMoves.AddRange(GenerateMoves(boardState, new Coord(i, j),checkLegal));

                   
                }
            }
            List<Move> legalMoves = new List<Move>();
            legalMoves = checkLegal ? pseudoLegalMoves.Where(x => IsLegal(x, boardState)).ToList() : pseudoLegalMoves;
           
            //Debug.Log("checkLegal:" + checkLegal);
            //Debug.Log(legalMoves.Count);
            //Debug.Log("Turn: " + boardState.turn);
            
            return legalMoves;
        }

        // This function generates all possible moves for the opponent on the current board       
        protected virtual List<Move> GenerateBoardMovesForOpponent(ChessState boardState, bool checkLegal) {
            List<Move> pseudoLegalMoves = new List<Move>();
            for (int i = 0; i < boardState.board.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.board.GetLength(1); j++)
                {
                    if (boardState.IsEmpty(i, j) || boardState.IsInTurn(boardState[i, j])) continue;

                    pseudoLegalMoves.AddRange(GenerateMovesForOpponent(boardState, new Coord(i,j),checkLegal));

                   
                }
            }
            List<Move> legalMoves = new List<Move>();
            legalMoves = checkLegal ? pseudoLegalMoves.Where(x => IsLegal(x, boardState)).ToList() : pseudoLegalMoves;
           
            //Debug.Log("checkLegal:" + checkLegal);
            //Debug.Log(legalMoves.Count);
            //Debug.Log("Turn: " + boardState.turn);
            
            return legalMoves;
        }

        public override bool IsCheck(ChessState state, Move move)
        {
            // Yeni durumu oluşturun
            ChessState newState = GenerateMoveSuccessor(state, move);
            
            // Rakibin tüm olası hamlelerini oluşturun
            List<Move> opponentMoves = GenerateBoardMovesForOpponent(newState, checkLegal: false);

            // Rakibin hamleleri arasında şahı tehdit eden hamle olup olmadığını kontrol edin
            foreach (var opponentMove in opponentMoves)
            {
                string piece = newState.GetPieceAt(opponentMove.targetCoord.rankIndex, opponentMove.targetCoord.fileIndex);
                if (piece != null && state.turn != newState.GetColor(piece) && piece[1] ==  KING_PIECE_NOTATION)
                {
                    Debug.Log("lllfffff");

                    return true; // Şah tehdit altında
                }
            }

            return false; // Şah tehdit altında değil
        }
        
        public override bool IsCheckForMe(ChessState state)
        {
            
            // Rakibin tüm olası hamlelerini oluşturun
            List<Move> opponentMoves = GenerateBoardMovesForOpponent(state, checkLegal: false);

            // Rakibin hamleleri arasında şahı tehdit eden hamle olup olmadığını kontrol edin
            foreach (var opponentMove in opponentMoves)
            {
                string piece = state.GetPieceAt(opponentMove.targetCoord.rankIndex, opponentMove.targetCoord.fileIndex);
                if (piece != null && state.turn == state.GetColor(piece) && piece[1] ==  KING_PIECE_NOTATION)
                {
                    Debug.Log("şşş");
                    return true; // Şah tehdit altında
                }
            }

            return false; // Şah tehdit altında değil
        }
        
        public override string GetVariantName() {
            return VARIANT_NAME;
        }

       
    }
}

