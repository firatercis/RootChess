using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftwareKingdom.Chess.Core;
using System.Linq;

namespace SoftwareKingdom.Chess.RootChess
{
    [CreateAssetMenu(fileName = "RootChessLogic", menuName = "SoftwareKingdom/Chess/RootChessLogic", order = 1)]

    public class RootChessLogic : BaseChessLogic
    {
        static char[] seedLetters =  BaseChessLogic.pieceLetters.Concat(new char[] { 'T', 'L' }).ToArray();


        public int[] growTurns =
        {
            1, // PAWN
            3, // KNIGHT
            3, // BISHOP
            5, // ROOK
            9, // QUEEN
            40, // KING
            3, // TIMER
            3, // PLANT
        };

        // Connections

        SeedPlantingMoveGenerator seedPlantingMoveGenerator;

        static string[,] initialRootChessBoard =
       {
            { null, null,null, null, "WK", null,null,null  },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { null,null,null,null,null,null,null,null },
            { null, null,null,null,null,null,null ,null },
            { null, null,null, null, "BK", null,null,null },
        };

        // Settings
    
        // Connections

        // State variables

        public RootChessLogic(List<string> flags = null) : base() // TODO: Seed variants
        {
        }

        public override ChessState CreateGame() {
            return CreateRootChessInitialState(null); // TODO: Parametre?
        }

        private static ChessState CreateRootChessInitialState(List<string> flags)
        {

            RootChessState initialBoardState = new RootChessState(initialRootChessBoard, flags);
            return initialBoardState;
        }

        public override List<Move> GenerateMoves(ChessState boardState, Coord sourceCoord, bool checkLegal = true) {
            List<Move> result ;
            if(sourceCoord.rankIndex == RootChessState.SEED_MOVES_SOURCE_RANK)
            {

                result = GenerateSeedMoves(
                    (RootChessState) boardState, 
                    sourceCoord.fileIndex + SeedPlantingMoveGenerator.MOVE_DEPLOY_PAWN_SEED);
            }
            else
            {
                result = base.GenerateMoves(boardState, sourceCoord);
            }

            return result;
        }


        //public override List<Move> GenerateMoves(ChessState boardState, Coord sourceCoord) {

        //    List<Move> result;   

        //    if(sourceCoord.rankIndex != RootChessState.SEED_MOVES_SOURCE_RANK)
        //    {
        //        result = base.GenerateMoves(boardState, sourceCoord);
        //    }
        //    else
        //    {
        //        result = GenerateSeedMoves((RootChessState)boardState,sourceCoord.fileIndex + SeedPlantingMoveGenerator.MOVE_DEPLOY_PAWN_SEED).ToList();
        //    }
        //    return result;
        //}

        protected List<Move> GenerateSeedMoves(RootChessState rootChessBoardState, int specificSeedType = SeedPlantingMoveGenerator.ALL_POSSIBLE_SEEDS) {
            string kingInTurn = rootChessBoardState.GetTurnPrefix().ToString() + RootChessState.KING_NOTATION;
            Coord kingPosition = rootChessBoardState.Search(kingInTurn);// TODO: Bunun tutulmasi

            List<Move> seedMoves = seedPlantingMoveGenerator.GenerateMoves(rootChessBoardState, kingPosition, specificSeedType).ToList();
            return seedMoves;
        }

       


        public virtual List<Move> GetPossibleSeedMoves(ChessState state, int seedDeployID) // KN: TODO: Selected seed
        {

            //RootChessMoveGenerator rootChessMoveGenerator = (RootChessMoveGenerator) moveGenerator;

            return GenerateSeedMoves(
                (RootChessState)state,
                seedDeployID
                ).ToList();
            // return moveGenerator.GeneratePieceMoves(boardState, sourceCoord);
            return null; // TODO: yapilacak burasi.
        }

      
        //public override void PlayMove(Move move)
        //{
        //    GrowSeeds();
        //    if (SeedPlantingMoveGenerator.IsSeedPlantingMove(move))
        //    {
        //        PlayPlantingMove(move);
        //    }
        //    else
        //    {
        //        base.PlayMove(move);
        //    }

        //    Debug.Log(boardState.ToString());

        //}

        private void PlayPlantingMove(ChessState state, Move move)
        {
            Debug.Log("Seed movement");
            int seedIndex = move.specialCondition - SeedPlantingMoveGenerator.MOVE_DEPLOY_PAWN_SEED;
            string seedNotation = state.GetTurnPrefix() + "S" + seedLetters[seedIndex] + growTurns[seedIndex];// Ex. WSB3 , White seed for bishop to grow in 3 turns
            state[move.targetCoord] = seedNotation;
          //  OnMovePlayed?.Invoke(move);
           // moveGenerator.SwitchTurn(boardState);
        }

        void GrowSeeds(ChessState state)
        {
            for(int i=0; i< state.board.GetLength(0); i++)
            {
                for (int j = 0; j < state.board.GetLength(1); j++)
                {
                    Coord currentCoord = new Coord(i, j);

                    if(state.GetPieceNotation(currentCoord) == RootChessState.SEED_NOTATION)
                    {
                        string grownSeed = GrowSeedPiece(state[currentCoord]);
                        state[currentCoord] = grownSeed;
                    }   
                }
            }
        }

        string GrowSeedPiece(string inPiece)
        {
            int growTurns = RootChessState.GetSeedPieceGrowTurns(inPiece);
            growTurns--;
            string output = "";
            if (growTurns > 0)
            {
                string piecePrefixWithoutNumber = RootChessState.GetSeedPieceWithoutTurns(inPiece);
                output = piecePrefixWithoutNumber + growTurns;
            }
            else
            {
                output = RootChessState.GetFullyGrownPiece(inPiece);

            }
            return output;

        }

    }

}

