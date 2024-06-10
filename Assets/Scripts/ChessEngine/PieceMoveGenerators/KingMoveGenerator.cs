using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace SoftwareKingdom.Chess.Core
{
    public class KingMoveGenerator: IPieceMoveGenerator
    {
        const char KING_PIECE_NOTATION = 'K';
        const char CASTLE_NOTATION = 'C';
        // Settings
        const int KING_N_DIRECTIONS = 8;
        // Settings
        Coord[] possibleDirections;

        // Connections

        // State variables

        public KingMoveGenerator()
        {
            possibleDirections = new Coord[KING_N_DIRECTIONS];
            possibleDirections[0] = new Coord(-1, 0);
            possibleDirections[1] = new Coord(1, 0);
            possibleDirections[2] = new Coord(0, -1);
            possibleDirections[3] = new Coord(0, 1);
            possibleDirections[4] = new Coord(-1, 1);
            possibleDirections[5] = new Coord(1, -1);
            possibleDirections[6] = new Coord(1, 1);
            possibleDirections[7] = new Coord(-1, -1);
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



                //if (boardState.IsEmpty(currentTargetPosition)) // Seed deployment move
                //{
                //    Move move = new Move(sourceCoord, currentTargetPosition, SpecialConditions.DeploySeed);
                //    moves.Add(move);
                //}

            }
            return moves.ToArray();
        }

        //public List<Move> GenerateCastlingMoves(ChessState boardState, Coord sourceCoord) {
        //    string ownPiece = boardState[sourceCoord];
        //    //List<Move> result = new List<Move>();
        //    //if (!boardState.IsInTurn(ownPiece))
        //    //{
        //    //    return result;
        //    //}
        //    //// Check king side castle

        //    //string kingSideCastleFlag = boardState.GetTurnPrefix() + CASTLE_NOTATION + "";


        //    //if(boardState.flags.Contains())

        //    //return result;
        //}

        public char GetPieceType()
        {
            return KING_PIECE_NOTATION;
        }
    }

}

