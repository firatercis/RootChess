
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftwareKingdom.Chess.Core;

namespace SoftwareKingdom.Chess.RootChess
{
    public class SeedPlantingMoveGenerator: IPieceMoveGenerator
    {

        public const int SEED_PIECE_NOTATION = 'S';

        public const int ALL_POSSIBLE_SEEDS = -1;

        public const int MOVE_DEPLOY_PAWN_SEED = 9;
        public const int MOVE_DEPLOY_KNIGHT_SEED = 10;
        public const int MOVE_DEPLOY_BISHOP_SEED = 11;
        public const int MOVE_DEPLOY_ROOK_SEED = 12;
        public const int MOVE_DEPLOY_QUEEN_SEED = 13;
        public const int MOVE_DEPLOY_KING_SEED = 14;
        public const int MOVE_DEPLOY_TIMER_SEED = 15;
        public const int MOVE_DEPLOY_PLANT_SEED = 16;


        const int SEEDING_N_DIRECTIONS = 8;
        // Settings

        // Connections
        Coord[] possibleDirections;
        // State variables

        public SeedPlantingMoveGenerator()
        {
            possibleDirections = new Coord[SEEDING_N_DIRECTIONS];
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
            return GenerateMoves(boardState, sourceCoord, ALL_POSSIBLE_SEEDS);
        }

        public Move[] GenerateMoves(ChessState boardState, Coord sourceCoord, int specificSeedType )
        {
            List<Move> moves = new List<Move>();
            string ownPiece = boardState[sourceCoord];
            for (int i = 0; i < possibleDirections.Length; i++)
            {
                Coord direction = possibleDirections[i];
                Coord currentTargetPosition = sourceCoord + direction;

                if (!boardState.IsInsideBoard(currentTargetPosition)) continue;

                if (boardState.IsEmpty(currentTargetPosition))
                {

                    if(specificSeedType == ALL_POSSIBLE_SEEDS)
                    {
                        moves.Add(new Move(sourceCoord, currentTargetPosition, MOVE_DEPLOY_PAWN_SEED));
                        moves.Add(new Move(sourceCoord, currentTargetPosition, MOVE_DEPLOY_KNIGHT_SEED));
                        moves.Add(new Move(sourceCoord, currentTargetPosition, MOVE_DEPLOY_BISHOP_SEED));
                        moves.Add(new Move(sourceCoord, currentTargetPosition, MOVE_DEPLOY_QUEEN_SEED));
                    }
                    else
                    {
                        moves.Add(new Move(sourceCoord, currentTargetPosition, specificSeedType));
                    }


                }



                //if (boardState.IsEmpty(currentTargetPosition)) // Seed deployment move
                //{
                //    Move move = new Move(sourceCoord, currentTargetPosition, SpecialConditions.DeploySeed);
                //    moves.Add(move);
                //}

            }
            return moves.ToArray();
        }

        public static bool IsSeedPlantingMove(Move move)
        {

            return move.specialCondition >= MOVE_DEPLOY_PAWN_SEED && move.specialCondition <= MOVE_DEPLOY_PLANT_SEED;
        }

        public char GetPieceType()
        {
            throw new System.NotImplementedException();
        }
    }
   }



