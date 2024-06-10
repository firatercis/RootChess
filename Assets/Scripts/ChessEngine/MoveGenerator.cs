using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SoftwareKingdom.Chess.Core
{
    public class MoveGenerator 
    {
        const char KING_PIECE_NOTATION = 'K';
        // Settings

        // Connections
        char[] pieceDefinitions;
        IPieceMoveGenerator[] pieceMoveGenerators; // TODO: List?
        // State variables
        Dictionary<char, IPieceMoveGenerator> moveGenDictionary;
        public MoveGenerator(char[] pieceDefinitions, IPieceMoveGenerator[] pieceMoveGenerators)
        {
            this.pieceMoveGenerators = pieceMoveGenerators;
            this.pieceDefinitions = pieceDefinitions;
            RegisterPieces();
        }

        void RegisterPieces()
        {
            moveGenDictionary = new Dictionary<char, IPieceMoveGenerator>();
            for (int i=0; i<pieceDefinitions.Length; i++)
            {
                moveGenDictionary.Add(pieceDefinitions[i], pieceMoveGenerators[i]);
            }
        }

        public virtual List<Move> GenerateBoardMoves(ChessState boardState)
        {
            return GenerateBoardMoves(boardState, checkLegal: true);
        }


       
        private List<Move> GenerateBoardMoves(ChessState boardState, bool checkLegal)
        {
            List<Move> pseudoLegalMoves = new List<Move>();   
            for(int i=0; i<boardState.board.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.board.GetLength(1); j++)
                {
                    if (boardState.IsEmpty(i, j) || !boardState.IsInTurn(boardState[i,j])) continue; 
                    IPieceMoveGenerator moveGen = moveGenDictionary.GetValueOrDefault(boardState.GetPieceNotation(new Coord(i,j)), null);
                    if (moveGen != null) {
                        Move[] pieceMoves = moveGen.GenerateMoves(boardState, new Coord(i, j));
                        pseudoLegalMoves.AddRange(pieceMoves);
                    }
                }
            }
            List<Move> legalMoves = checkLegal ? pseudoLegalMoves.Where(x => IsLegal(x, boardState)).ToList() : pseudoLegalMoves;

            return legalMoves;
        }

        public virtual bool IsLegal(Move move, ChessState boardState)
        {
            ChessState nextBoardState = GenerateMoveSuccessor(boardState, move);
            List<Move> opponentMoves = GenerateBoardMoves(nextBoardState, checkLegal: false);

            bool result = true;
            for(int i=0; i<opponentMoves.Count; i++)
            {
                if (nextBoardState.IsEmpty(opponentMoves[i].targetCoord))continue; 

                if(nextBoardState.GetPieceNotation( opponentMoves[i].targetCoord) == KING_PIECE_NOTATION) // Check if some enemy piece can take a king
                {
                    result = false;
                    break;
                } 
            }
            return result;
        }


        public ChessState GenerateMoveSuccessor(ChessState inputState, Move move)
        {
            ChessState outputState = new ChessState(inputState);
            ApplyMove(move, outputState);
            return outputState;
        }

        public virtual void ApplyMove(Move move, ChessState boardState)
        {
           
            boardState[move.targetCoord] = boardState[move.startCoord];
            boardState[move.startCoord] = ChessState.EMPTY_SQUARE;           
            SwitchTurn(boardState);
        }

        public virtual void SwitchTurn(ChessState boardState)
        {
            boardState.turn = (boardState.turn + 1) % ChessState.N_SIDES;
        }

        
        public char GetPieceType()
        {
            throw new System.NotImplementedException();
        }
    }
}


