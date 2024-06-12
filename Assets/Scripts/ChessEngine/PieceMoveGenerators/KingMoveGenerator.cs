using System.Security.AccessControl;
using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


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

                Move[] castleMoves = GenerateCastlingMoves(boardState, sourceCoord);

                for(int j = 0; j < castleMoves.Length; j++){
                    moves.Add(castleMoves[j]);
                }

                


                //if (boardState.IsEmpty(currentTargetPosition)) // Seed deployment move
                //{
                //    Move move = new Move(sourceCoord, currentTargetPosition, SpecialConditions.DeploySeed);
                //    moves.Add(move);
                //}

            }
            return moves.ToArray();
        }

        
        public Move[] GenerateCastlingMoves(ChessState boardState, Coord sourceCoord)
        {
            string ownPiece = boardState[sourceCoord];
            List<Move> result = new List<Move>();
            Coord kingSideMoveDirection =  new Coord(0, +1);
            Coord queenSideMoveDirection =  new Coord(0, -1);
           

            if (!boardState.IsInTurn(ownPiece))
            {
                return result.ToArray();
            }


            string kingSideCastleFlag = boardState.GetTurnPrefix() + CASTLE_NOTATION + "0-0";
            string queenSideCastleFlag = boardState.GetTurnPrefix() + CASTLE_NOTATION + "0-0-0";

            if(boardState.flags.Contains("WC0-0")){
                if(sourceCoord == new Coord(0,4)){
                    if(boardState.IsEmpty(sourceCoord + kingSideMoveDirection)){
                        if(boardState.IsEmpty(sourceCoord + kingSideMoveDirection + kingSideMoveDirection))
                        {
                            Move kingSideCastleKingMoveWhite = new Move(sourceCoord, sourceCoord + kingSideMoveDirection + kingSideMoveDirection);
                            kingSideCastleKingMoveWhite.specialCondition = SpecialConditions.Castling;
                            result.Add(kingSideCastleKingMoveWhite);
                        }
                    }
                }
                
            }
            if(boardState.flags.Contains("BC0-0")){
                if(sourceCoord == new Coord(7,4)){
                    if(boardState.IsEmpty(sourceCoord + kingSideMoveDirection)){
                        if(boardState.IsEmpty(sourceCoord + kingSideMoveDirection + kingSideMoveDirection))
                        {
                            Move kingSideCastleKingMoveBlack = new Move(sourceCoord, sourceCoord + kingSideMoveDirection + kingSideMoveDirection);
                            kingSideCastleKingMoveBlack.specialCondition = SpecialConditions.Castling;

                            result.Add(kingSideCastleKingMoveBlack);
                        }
                    }
                }
            }
          

            if(boardState.flags.Contains("WC0-0-0")){
                if(sourceCoord == new Coord(0,4)){
                    if(boardState.IsEmpty(sourceCoord + queenSideMoveDirection)){
                        if(boardState.IsEmpty(sourceCoord + queenSideMoveDirection + queenSideMoveDirection)){
                            if(boardState.IsEmpty(sourceCoord + queenSideMoveDirection + queenSideMoveDirection + queenSideMoveDirection))
                            {
                                Move queenSideCastleKingMoveWhite = new Move(sourceCoord, sourceCoord + queenSideMoveDirection + queenSideMoveDirection);
                                queenSideCastleKingMoveWhite.specialCondition = SpecialConditions.Castling;
                                result.Add(queenSideCastleKingMoveWhite);
                            }
                        }
                    }
                }
            }

            if(boardState.flags.Contains("BC0-0-0")){
                if(sourceCoord == new Coord(7,4)){
                    if(boardState.IsEmpty(sourceCoord + queenSideMoveDirection)){
                        if(boardState.IsEmpty(sourceCoord + queenSideMoveDirection + queenSideMoveDirection)){
                            if(boardState.IsEmpty(sourceCoord + queenSideMoveDirection + queenSideMoveDirection + queenSideMoveDirection))
                            {
                                Move queenSideCastleKingMoveBlack = new Move(sourceCoord, sourceCoord + queenSideMoveDirection + queenSideMoveDirection);
                                queenSideCastleKingMoveBlack.specialCondition = SpecialConditions.Castling;

                                result.Add(queenSideCastleKingMoveBlack);
                            }
                        }
                    }
                }
            }
            
            return result.ToArray();
            
        }

        
        

        public char GetPieceType()
        {
            return KING_PIECE_NOTATION;
        }
    }

}

