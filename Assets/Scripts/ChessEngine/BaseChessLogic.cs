using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SoftwareKingdom.Chess.Core
{

    [CreateAssetMenu(fileName = "StandardChessLogic", menuName = "SoftwareKingdom/Chess/StandardChessLogic", order = 1)]
    public class BaseChessLogic : ChessLogic 
    {
        const string VARIANT_NAME = "Standard";


        const char BISHOP_PIECE_NOTATION = 'B';
        const char QUEEN_PIECE_NOTATION = 'Q';
        const char ROOK_PIECE_NOTATION = 'R';
        const char KING_PIECE_NOTATION = 'K';
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

        

            return checkLegal ? result.Where(x => IsLegal(x, boardState)).ToList() : result;
        }

        public virtual bool IsLegal(Move move, ChessState boardState) {
            ChessState nextBoardState = GenerateMoveSuccessor(boardState, move);
            List<Move> opponentMoves = GenerateBoardMoves(nextBoardState, checkLegal: false);

            bool result = true;
            for (int i = 0; i < opponentMoves.Count; i++)
            {
                if (nextBoardState.IsEmpty(opponentMoves[i].targetCoord)) continue;

                if (nextBoardState.GetPieceNotation(opponentMoves[i].targetCoord) == KING_PIECE_NOTATION) // Check if some enemy piece can take a king
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public virtual ChessState GenerateMoveSuccessor(ChessState inputState, Move move) {
            ChessState outputState = new ChessState(inputState);
            ApplyMove( outputState, move);
            return outputState;
        }

        public override void ApplyMove( ChessState boardState, Move move) {

            boardState[move.targetCoord] = boardState[move.startCoord];
            boardState[move.startCoord] = ChessState.EMPTY_SQUARE;


            // Change en passant state
            SaveEnPassantCoord(boardState, move);
            CheckApplyEnPassant(boardState, move);
            SwitchTurn(boardState);

        }

        private static void CheckApplyEnPassant(ChessState boardState, Move move) {
            if (move.specialCondition == SpecialConditions.EnPassantCapture)
            {
                int oneBackward = -1;
                if (boardState.turn == ChessState.BLACK)
                {
                    oneBackward =1 ;
                }

                boardState[move.targetCoord + new Coord(oneBackward, 0)] = ChessState.EMPTY_SQUARE;
            }
        }

        private static void SaveEnPassantCoord(ChessState boardState, Move move) {
            if (move.specialCondition != SpecialConditions.PawnTwoForward)
            {
                boardState.enPassantCoord = ChessState.NO_EN_PASSANT_COORD;
            }
            else
            {
                int enPassantRankIndex = (move.targetCoord.rankIndex + move.startCoord.rankIndex) / 2;
                int enPassantFileIndex = move.targetCoord.fileIndex;
                boardState.enPassantCoord = new Coord(enPassantRankIndex, enPassantFileIndex);
            }
        }


        public  override List<Move> GenerateBoardMoves(ChessState boardState) { // TODO: Can be virtual
            return GenerateBoardMoves(boardState, checkLegal: true);
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
            List<Move> legalMoves = checkLegal ? pseudoLegalMoves.Where(x => IsLegal(x, boardState)).ToList() : pseudoLegalMoves;

            return legalMoves;
        }


        public override string GetVariantName() {
            return VARIANT_NAME;
        }

       
    }
}

