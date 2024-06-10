using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace SoftwareKingdom.Chess.Core
{
    public class KnightMovesGenerator : IPieceMoveGenerator
    {
        const char KNIGHT_PIECE_NOTATION = 'N';
        const int KNIGHT_N_DIRECTIONS = 8;

        // Settings
        private Coord[] possibleDirections = { };
        // Connections
        // State variables

        public KnightMovesGenerator()
        {
            possibleDirections = new Coord[KNIGHT_N_DIRECTIONS];
            possibleDirections[0] = new Coord(-2, 1);
            possibleDirections[1] = new Coord(2, 1);
            possibleDirections[2] = new Coord(-2, -1);
            possibleDirections[3] = new Coord(2, -1);
            possibleDirections[4] = new Coord(1, 2);
            possibleDirections[5] = new Coord(1, -2);
            possibleDirections[6] = new Coord(-1, 2);
            possibleDirections[7] = new Coord(-1, -2);
        }

        public Move[] GenerateMoves(ChessState boardState, Coord sourceCoord)
        {
            List<Move> moves = new List<Move>();
            string ownPiece = boardState[sourceCoord];
            for (int i = 0; i < possibleDirections.Length; i++)
            {
                Coord direction = possibleDirections[i];
                Coord currentTargetPosition = sourceCoord + direction;

                if (!boardState.IsInsideBoard(currentTargetPosition)) continue;

                if (boardState.CanLandOn(ownPiece, boardState[currentTargetPosition]))
                {
                    Move move = new Move(sourceCoord, currentTargetPosition);
                    moves.Add(move);
                }
            }
            return moves.ToArray();
        }

        public char GetPieceType()
        {
            return KNIGHT_PIECE_NOTATION;
        }
    }
}
