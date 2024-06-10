namespace SoftwareKingdom.Chess.Core
{
    public interface IPieceMoveGenerator{

        char GetPieceType();

        Move[] GenerateMoves(ChessState boardState,Coord sourceCoord);
    }
}


