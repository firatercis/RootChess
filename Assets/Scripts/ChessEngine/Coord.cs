namespace SoftwareKingdom.Chess.Core
{
    public struct Coord
    {
        public int rankIndex;
        public int fileIndex;



        public Coord(int i, int j)
        {
            this.rankIndex = i;
            this.fileIndex = j;
        }

        static public Coord operator +(Coord v1, Coord v2)
        {
            Coord result;
            result.rankIndex = v1.rankIndex + v2.rankIndex;
            result.fileIndex = v1.fileIndex + v2.fileIndex;
            return result;
        }


        public bool IsLightSquare()
        {
            return (fileIndex + rankIndex) % 2 != 0;
        }

        public int CompareTo(Coord other)
        {
            return (fileIndex == other.fileIndex && rankIndex == other.rankIndex) ? 0 : 1;
        }
    }

}

