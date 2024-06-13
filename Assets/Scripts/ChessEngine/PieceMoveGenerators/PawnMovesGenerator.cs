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

                // checks if the pawn promotes
                // only promotes to a queen 
                if (oneForward.rankIndex == lastRank){
                    oneForwardMove.specialCondition = SpecialConditions.PromoteToQueen; // Special condition is set as promoting to queen
                    // oneForwardMove.specialCondition = SpecialConditions.PromoteToKnight; // Other promotions
                    // oneForwardMove.specialCondition = SpecialConditions.PromoteToBishop; // Other promotions
                    // oneForwardMove.specialCondition = SpecialConditions.PromoteToRook; // Other promotions

                }

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

            // Left diagonal
            Coord leftDiagonal = oneForward + new Coord(0, 1);
            if (boardState.IsInsideBoard(leftDiagonal))
            {
                // Left diagonal capture move
                if (boardState.IsEnemy(ownPiece, boardState[leftDiagonal]))
                {
                    
                    Move leftDiagonalTakeMove = new Move(sourceCoord, leftDiagonal);

                    // checks if the pawn promotes
                    // only promotes to a queen
                    if (leftDiagonal.rankIndex == lastRank){
                        leftDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToQueen; // Special condition is set as promoting to queen
                        // leftDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToKnight; // Other promotions
                        // leftDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToBishop; // Other promotions
                        // leftDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToRook; // Other promotions
                    }
                    moves.Add(leftDiagonalTakeMove);
                }

                // Left diagonal En Passant capture move 
                if (leftDiagonal == boardState.enPassantCoord) // Left diagonal En Passant check
                {
                    Move leftDiagonalEnPassantTakeMove = new Move(sourceCoord, leftDiagonal);
                    leftDiagonalEnPassantTakeMove.specialCondition = SpecialConditions.EnPassantCapture;
                    moves.Add(leftDiagonalEnPassantTakeMove); // Adds left diagonal En Passant capture to possible moves
                }
                
                

            }

            // Right diagonal
            Coord rightDiagonal = oneForward + new Coord(0, -1);
            if (boardState.IsInsideBoard(rightDiagonal))
            {
                // Right diagonal capture move
                if (boardState.IsEnemy(ownPiece, boardState[rightDiagonal]))
                {
                    Move rightDiagonalTakeMove = new Move(sourceCoord, rightDiagonal);

                    // checks if the pawn promotes
                    // only promotes to a queen
                    if (rightDiagonal.rankIndex == lastRank){
                        rightDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToQueen; // Special condition is set as promoting to queen
                        // rightDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToKnight; // Other promotions
                        // rightDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToBishop; // Other promotions
                        // rightDiagonalTakeMove.specialCondition = SpecialConditions.PromoteToRook; // Other promotions
                    }
                    moves.Add(rightDiagonalTakeMove);
                }

                // Right diagonal En Passant capture move 
                if(rightDiagonal == boardState.enPassantCoord) // Right diagonal En Passant check
                {
                    Move rightDiagonalEnPassantTakeMove = new Move(sourceCoord, rightDiagonal);
                    rightDiagonalEnPassantTakeMove.specialCondition = SpecialConditions.EnPassantCapture;
                    moves.Add(rightDiagonalEnPassantTakeMove); // Adds right diagonal En Passant capture to possible moves
                }

            }


            return moves.ToArray();
        }

        public char GetPieceType()
        {
            return PAWN_PIECE_NOTATION;
        }
    }
}


