using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;

namespace SoftwareKingdom.Chess.RootChess
{
    public class RootChessMoveGenerator : MoveGenerator
    {


        // Settings

        // Connections
        SeedPlantingMoveGenerator seedPlantingMoveGenerator;
        // State variables
        public RootChessMoveGenerator(char[] pieceDefinitions, IPieceMoveGenerator[] pieceMoveGenerators) : base(pieceDefinitions, pieceMoveGenerators)
        {
            seedPlantingMoveGenerator = new SeedPlantingMoveGenerator(); // TODO: Seed planting move gen settings
        }

        public override List<Move> GenerateBoardMoves(ChessState boardState)
        {
            List<Move> possibleMoves = base.GenerateBoardMoves(boardState);
            Move[] seedMoves = GenerateSeedMoves((RootChessState)boardState);
            possibleMoves.AddRange(seedMoves);
            return possibleMoves;
        }

        public Move[] GenerateSeedMoves(RootChessState rootChessBoardState, int specificSeedType = SeedPlantingMoveGenerator.ALL_POSSIBLE_SEEDS)
        {
            string kingInTurn = rootChessBoardState.GetTurnPrefix().ToString() + RootChessState.KING_NOTATION;
            Coord kingPosition = rootChessBoardState.Search(kingInTurn);// TODO: Bunun tutulmasi

            Move[] seedMoves = seedPlantingMoveGenerator.GenerateMoves(rootChessBoardState, kingPosition, specificSeedType);
            return seedMoves;
        }

        

    }

}

