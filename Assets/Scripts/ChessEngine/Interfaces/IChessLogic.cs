


using System.Collections.Generic;

namespace SoftwareKingdom.Chess.Core
{
    public interface IChessLogic{

        public string GetVariantName();
        public List<Move> GenerateMoves(ChessState boardState, Coord sourceCoord,bool checkLegal = true);
        public List<Move> GenerateBoardMoves(ChessState boardState);
        public void ApplyMove(ChessState boardState,Move move);

        public ChessState CreateGame();
    
    }
}


