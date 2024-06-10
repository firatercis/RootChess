/* 
To preserve memory during search, moves are stored as 16 bit numbers.
The format is as follows:

bit 0-5: from square (0 to 63)
bit 6-11: to square (0 to 63)
bit 12-15: flag
*/
namespace SoftwareKingdom.Chess.Core {

    public class SpecialConditions
    {
        public const int None = 0;
        public const int EnPassantCapture = 1;
        public const int Castling = 2;
        public const int PromoteToQueen = 3;
        public const int PromoteToKnight = 4;
        public const int PromoteToRook = 5;
        public const int PromoteToBishop = 6;
        public const int PawnTwoForward = 7;
        public const int DeploySeed = 8; // Added by fercis

    }

	public struct Move {
		public Coord startCoord;
		public Coord targetCoord;
		public int specialCondition ; // One of the special conditions

        public Move(Coord startCoord, Coord targetCoord, int specialCondition = SpecialConditions.None)
        {
            this.startCoord = startCoord;
            this.targetCoord = targetCoord;
            this.specialCondition = specialCondition;
        }


        public bool IsSeedMove() //TODO: kaldirilacak.
        {
            return specialCondition == SpecialConditions.DeploySeed;
        }

	}
}
