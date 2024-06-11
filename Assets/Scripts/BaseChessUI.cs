using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using System;



namespace SoftwareKingdom.Chess.UI
{



    public class BaseChessUI : MonoBehaviour, SquareClickListener
    {

        // Settings
        public Vector3 horizontalOffset;
        public Vector3 verticalOffset;
        public Color lightSquaresColor;
        public Color darkSquaresColor;
        // Connections
        public ChessPlayer[] players;
        public ChessLogic logic; 
        public Transform startPoint;
        public BoardSquare[] squaresPool;
        Dictionary<Coord, BoardSquare> coordSquareDictionary;
   
        PieceLiner pieceLiner;
        public SeedSelectionPanel seedSelectionPanel; // TODO: Normal chess UI ile abstraction
        // State variables
        int nextSquareIndex;
        bool waitingForHumanInput;
        bool pieceSelected;
        List<Move> selectedPossibleMoves;
      
        void Awake()
        {
            InitConnections();
        }

        void Start()
        {
            InitState();
        }

        void InitConnections()
        {
            coordSquareDictionary = new Dictionary<Coord, BoardSquare>();// TODO: Is this a good approach?
            pieceLiner = GetComponent<PieceLiner>();
            logic.OnMovePlayed += MakeMove;
            logic.OnTurn += OnTurn;
        }

        private void OnTurn(int turnIndex) {
            if (players[turnIndex].IsHuman())
            {
                waitingForHumanInput = true;
            }
            else
            {
                players[turnIndex].OnTurn(logic.GetState());
            }
        }

        void InitState()
        {
            logic.CreateGame();
            waitingForHumanInput = false;
            logic.StartGame();
        }

        public void OnSquareClick(int rankIndex, int fileIndex) {
            if (!waitingForHumanInput) return; // If it is not your turn, do nothing

            if (!pieceSelected)
            {
                UnHighlight();
                selectedPossibleMoves = logic.GenerateMoves(new Coord(rankIndex, fileIndex));
                if (selectedPossibleMoves.Count > 0)
                {
                    HighlightMoves(selectedPossibleMoves);
                    pieceSelected = true;
                    // TODO: Piece selected effect
                }
            }
            else
            {
                Move[] selectedMoves = selectedPossibleMoves.Where(x => x.targetCoord.fileIndex == fileIndex && x.targetCoord.rankIndex == rankIndex).ToArray();
                if (selectedMoves.Length > 0)
                {
                    waitingForHumanInput = false;
                    logic.PlayMove(selectedMoves[0]);
                }
                UnHighlight();
                pieceSelected = false;
            }
        }


        public void CreateBoard(ChessState initialState)
        {
            nextSquareIndex = 0;

            for(int i=0; i < initialState.board.GetLength(0); i++)
            {
                for(int j=0; j < initialState.board.GetLength(1); j++)
                {
                    // You may check initialState[i,j] for some holes
                    Vector3 position = CoordToPosition(i,j);
                    Color squareColor = (i + j) % 2 == 0 ? darkSquaresColor : lightSquaresColor;
                    squaresPool[nextSquareIndex].Configure(i, j, squareColor, position, this);
                    Coord coord = new Coord(i, j);  
                    coordSquareDictionary.Add(coord, squaresPool[nextSquareIndex]);

                    // Draw the piece
                    if (!initialState.IsEmpty(i, j)) 
                        pieceLiner.DrawPiece(initialState[coord], squaresPool[nextSquareIndex].pieceNest.position, coord);
                    nextSquareIndex++;
                }
            }
        }

        public void MakeMove(Move move)
        {
            Vector3 position = GetPiecePosition(move.targetCoord);
            if( !SeedPlantingMoveGenerator.IsSeedPlantingMove(move))
            {
                pieceLiner.MovePiece(move.startCoord, move.targetCoord, position);

                if(move.specialCondition == SpecialConditions.EnPassantCapture)
                {
                    Coord enPassantCoord = new Coord(move.startCoord.rankIndex, move.targetCoord.fileIndex);
                    pieceLiner.CheckClearSquare(enPassantCoord);
                }

            }
            else
            {
                ChessState boardState = logic.GetState();

                RootChessState rootChessBoardState = (RootChessState)boardState;
                char pieceLetter = rootChessBoardState.GetSeedPieceNotation(move.targetCoord);
                string pieceNotation = "" +rootChessBoardState.GetTurnPrefix() + pieceLetter; // TODO: Cok dolambacli yol
                int seedTurns = rootChessBoardState.GetSeedPieceGrowTurns(move.targetCoord);
                pieceLiner.AddSeed(pieceNotation.ToString(),  position, move.targetCoord, seedTurns);
                Debug.Log("Grow seed: " + move.specialCondition + " at " + move.targetCoord);
            }
        }

        public void UpdateSeeds(ChessState boardState)
        {
            RootChessState rootChessBoardState = (RootChessState)boardState;

            for(int i=0; i< boardState.board.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.board.GetLength(1); j++)
                {

                    //char pieceLetter = rootChessBoardState.GetSeedPieceNotation(move.targetCoord);
                }
            }

            //string pieceNotation = "" + rootChessBoardState.GetTurnPrefix() + pieceLetter; // TODO: Cok dolambacli yol
            //int seedTurns = rootChessBoardState.GetSeedPieceGrowTurns(move.targetCoord);
            //pieceLiner.AddSeed(pieceNotation.ToString(), position, move.targetCoord, seedTurns);
            //Debug.Log("Grow seed: " + move.specialCondition + " at " + move.targetCoord);
        }



        public void Highlight(int rankIndex, int fileIndex)
        {
            BoardSquare square = coordSquareDictionary[new Coord(rankIndex, fileIndex)];
            square.Highlight();
        }

        public void HighlightMoves(List<Move> moves)
        {
            for(int i=0; i<moves.Count; i++)
            {
                Highlight(moves[i].targetCoord.rankIndex, moves[i].targetCoord.fileIndex);
            }
        }

        public void UnHighlight()
        {

            foreach (KeyValuePair<Coord, BoardSquare> entry in coordSquareDictionary)
            {
                entry.Value.UnHighlight();
            }
        }



        #region Appendix Functions

        private Vector3 CoordToPosition(int i, int j)
        {
            return startPoint.position + (i * verticalOffset) + (j * horizontalOffset); 
        }

        private Vector3 GetPiecePosition(Coord coord)
        {
            BoardSquare square = coordSquareDictionary[coord];
            return square.pieceNest.position;
        }

        #endregion

        // Update is called once per frame
        void Update()
        {

        }

       
    }

}

