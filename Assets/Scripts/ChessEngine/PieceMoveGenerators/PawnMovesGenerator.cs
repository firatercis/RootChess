using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace SoftwareKingdom.Chess.Core
{
    public class PawnMovesGenerator : IPieceMoveGenerator
    {
        const char PAWN_PIECE_NOTATION = 'P';
        // Settings

        // Connections

        // State variables


        public Move[] GenerateMoves(ChessState boardState, Coord sourceCoord)
        {
            List<Move> moves = new List<Move>();
            string ownPiece = boardState[sourceCoord];
            int side = boardState.GetColor(ownPiece);
            Coord moveDirection = side == ChessState.WHITE ? new Coord(1, 0) : new Coord(-1, 0);
            int startRank = side == ChessState.WHITE ? 1 : 6; // TODO: Magic number
            int lastRank = side == ChessState.WHITE ? 7 : 0;
            int finalRankBeforePromotion = side == ChessState.WHITE ? 6 : 1;

            // one forward position
            Coord oneForward = sourceCoord + moveDirection; //TODO: Tahtadan cikma kontrolu
            if (boardState.IsEmpty(oneForward))
            {
                Move oneForwardMove = new Move(sourceCoord, oneForward);

                if (oneForward.rankIndex == lastRank)
                    oneForwardMove.specialCondition = SpecialConditions.PromoteToQueen;

                moves.Add(oneForwardMove);
            }
            // two forward position
            Coord twoForward = oneForward + moveDirection;
            if (sourceCoord.rankIndex == startRank && boardState.IsEmpty(oneForward) && boardState.IsEmpty(twoForward))
            {
                Move twoForwardMove = new Move(sourceCoord, twoForward);
                twoForwardMove.specialCondition = SpecialConditions.PawnTwoForward;
                moves.Add(twoForwardMove);
            }

            // One point diagonals
            Coord leftDiagonal = oneForward + new Coord(0, 1);
            if (boardState.IsInsideBoard(leftDiagonal))
            {
                if (boardState.IsEnemy(ownPiece, boardState[leftDiagonal]))
                {
                    Move leftDiagonalTakeMove = new Move(sourceCoord, leftDiagonal);
                    moves.Add(leftDiagonalTakeMove);
                }

                if (leftDiagonal == boardState.enPassantCoord)
                {
                    Move leftDiagonalTakeMove = new Move(sourceCoord, leftDiagonal);
                    leftDiagonalTakeMove.specialCondition = SpecialConditions.EnPassantCapture;
                    moves.Add(leftDiagonalTakeMove);
                }

            }
            Coord rightDiagonal = oneForward + new Coord(0, -1);
            if (boardState.IsInsideBoard(rightDiagonal))
            {
                if (boardState.IsEnemy(ownPiece, boardState[rightDiagonal]))
                {
                    Move rightDiagonalTakeMove = new Move(sourceCoord, rightDiagonal);
                    moves.Add(rightDiagonalTakeMove);
                }

                if(rightDiagonal == boardState.enPassantCoord)
                {
                    Move rightDiagonalTakeMove = new Move(sourceCoord, rightDiagonal);
                    rightDiagonalTakeMove.specialCondition = SpecialConditions.EnPassantCapture;
                    moves.Add(rightDiagonalTakeMove);
                }

            }
            
            
            // En passant?  TODO:


            return moves.ToArray();
        }

        public char GetPieceType()
        {
            return PAWN_PIECE_NOTATION;
        }
    }
}


