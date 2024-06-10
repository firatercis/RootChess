using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoftwareKingdom.Chess.Core
{
    public class SlidingPieceMovesGenerator : IPieceMoveGenerator
    {

        // Settings
        char pieceNotation;
        // Connections
        protected Coord[] possibleDirections = { };
        // State variables

        public SlidingPieceMovesGenerator(Coord[] possibleDirections, char pieceNotation)
        {
            this.possibleDirections = possibleDirections;
            this.pieceNotation = pieceNotation;
        }

        public Move[] GenerateMoves(ChessState boardState, Coord sourceCoord)
        {
            List<Move> moves = new List<Move>();
            string ownPiece = boardState[sourceCoord];

            for (int i = 0; i < possibleDirections.Length; i++)
            {
                Coord direction = possibleDirections[i];
                Coord currentTargetPosition = sourceCoord;
                while (true)
                {
                    currentTargetPosition += direction;

                    if (!boardState.IsInsideBoard(currentTargetPosition)) break;

                    if (boardState.CanLandOn(ownPiece, boardState[currentTargetPosition])) // If blank or a stranger, you can move there
                    {
                        Move move = new Move(sourceCoord, currentTargetPosition);
                        moves.Add(move);
                    }
                    if (!boardState.CanWalkOver(ownPiece, boardState[currentTargetPosition]))
                        break;
                }
            }
            return moves.ToArray();
        }

        public char GetPieceType()
        {
            return pieceNotation;
        }
    }

}

