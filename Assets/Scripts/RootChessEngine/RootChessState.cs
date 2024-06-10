using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SoftwareKingdom.Chess.Core;



namespace SoftwareKingdom.Chess.RootChess
{
    public class RootChessState : ChessState
    {

        public const int SEED_MOVES_SOURCE_RANK = -1;

        public const int SEED_PIECE_NOTATION_INDEX = 2;
        public const int SEED_TURN_NOTATION_INDEX = 3;
        // Constants
        public const char KING_NOTATION = 'K';
        public const char SEED_NOTATION = 'S';
        // Settings

        // Connections

        // State variables
        public int nSeedsWhite;
        public int nSeedsBlack;
        
        public RootChessState(int width, int height) 
            : base(width, height)
        {
        }

        public RootChessState(string[,] board, List<string> flags) 
            : base(board, flags)
        {

        }


        public char GetSeedPieceNotation(Coord coord)
        {
            string piece = board[coord.rankIndex, coord.fileIndex];
            return GetSeedPieceNotation(piece);
        }
        public static char GetSeedPieceNotation(string piece)
        {
            return piece[SEED_PIECE_NOTATION_INDEX];
        }



        public int GetSeedPieceGrowTurns(Coord coord)
        {
            string piece = board[coord.rankIndex, coord.fileIndex];
            return GetSeedPieceGrowTurns(piece);
        }

        public static int GetSeedPieceGrowTurns(string pieceNotation)
        {
            var digits = pieceNotation.Reverse().TakeWhile(c => char.IsDigit(c));
            var number = new string(digits.Reverse().ToArray());
            return int.Parse(number);
        }

        public static string GetSeedPieceWithoutTurns(string pieceNotation)
        {
            var letters = pieceNotation.TakeWhile(c => !char.IsDigit(c));
            return new string(letters.ToArray());
        }

        public static string GetFullyGrownPiece(string seedPiece) //TODO cok karisik burasi
        {
            char piecePrefix = seedPiece[0]; // TODO: COLOR_PREFIX
            char pieceNotation = GetSeedPieceNotation(seedPiece);
            string result = "" + piecePrefix + pieceNotation;
            return result;
        }
        

        public override bool CanWalkOver(string piece, string target)
        {
            return target == EMPTY_SQUARE || target[PIECE_NOTATION_INDEX] == SEED_NOTATION;
        }
    }

}
