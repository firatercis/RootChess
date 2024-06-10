using SoftwareKingdom.Chess.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SoftwareKingdom.Chess.Core {

    public class MoveRecord{
        public Move appliedMove;
        public string takenPiece;
    }

    //public class ChessGame
    //{
    //    // Settings
    //    public string variantName;
    //    // Connections
    //    protected IChessLogic chessLogic;
    //    public Action<Move> OnMovePlayed;
    //    public IPieceMoveGenerator[] moveGenerators;
    //    public IMoveApplication[] moveApplications;
    //    // State variables

    //    public ChessState boardState;
    //    public List<MoveRecord> moveRecords;
    //    public Dictionary<int, IMoveApplication> moveApplicationsDict;
    //    public Dictionary<char, IPieceMoveGenerator> moveGeneratorsDict;

    //    public  ChessGame(ChessState initialState, IChessLogic chessLogic)
    //    {
    //        boardState = initialState;
    //        //this.moveGenerator = moveGenerator;
    //        moveRecords = new List<MoveRecord>();
    //        this.moveGenerators = moveGenerators;
    //       // this.moveApplications = moveApplications

    //    }

    //    private void RegisterMoveApplications()
    //    {
    //        moveApplicationsDict = new Dictionary<int, IMoveApplication>();
    //        for(int i=0; i< moveApplications.Length; i++)
    //        {
    //            moveApplicationsDict.Add(moveApplications[i].GetMoveType(), moveApplications[i]);
    //        }
    //    }

      

    //    public virtual void PlayMove(Move move)
    //    {
    //        //moveGenerator.ApplyMove(move,boardState);
    //        IMoveApplication moveApplication = moveApplicationsDict[move.specialCondition];
    //        moveApplication.ApplyMove(move, boardState);

    //        MoveRecord moveRecord = new MoveRecord();
    //        moveRecord.takenPiece = boardState[move.targetCoord];
    //        moveRecords.Add(moveRecord);
    //        OnMovePlayed?.Invoke(move);
    //    }

        

    //    public void SwitchTurn(ChessState boardState)
    //    {
    //        boardState.turn = (boardState.turn + 1) % ChessState.N_SIDES;
    //    }



    //    public void UndoMove(Move move) // TODO: undo move from the list and stack, maybe generate a new boardState
    //    {

    //    }

    //    public virtual List<Move> GetPossibleMoves(Coord sourceCoord)
    //    {
    //        List<Move> moves = new List<Move>();
    //        if (boardState.IsEmpty(sourceCoord) || !boardState.IsInTurn(boardState[sourceCoord])) return moves;

    //        IPieceMoveGenerator pieceMoveGenerator = moveGeneratorsDict[boardState.GetPieceNotation(sourceCoord)];

    //        if (pieceMoveGenerator != null)
    //        {
    //            moves.AddRange(pieceMoveGenerator.GenerateMoves(boardState, sourceCoord));
    //        }

    //        // Filter to legal moves
    //        moves = moves.Where(x => IsLegal(x, boardState)).ToList();

    //        return moves;

    //        //return moveGenerator.GeneratePieceMoves(boardState, sourceCoord);
    //    }

    //    public ChessState GenerateMoveSuccessor(ChessState inputState, Move move)
    //    {
    //        IMoveApplication moveApplication = moveApplicationsDict[move.specialCondition];
    //        moveApplication.ApplyMove(move, inputState);
    //        ChessState outputState = new ChessState(inputState);
    //        return outputState;
    //    }

    //    public bool IsLegal(Move move, ChessState boardState)
    //    {
    //        ChessState nextBoardState = GenerateMoveSuccessor(boardState, move);
    //        List<Move> opponentMoves = GenerateBoardMoves(nextBoardState, checkLegal: false);

    //        bool result = true;
    //        for (int i = 0; i < opponentMoves.Count; i++)
    //        {
    //            if (nextBoardState.IsEmpty(opponentMoves[i].targetCoord)) continue;

    //            if (nextBoardState.GetPieceNotation(opponentMoves[i].targetCoord) == KING_PIECE_NOTATION) // Check if some enemy piece can take a king
    //            {
    //                result = false;
    //                break;
    //            }
    //        }
    //        return result;
    //    }



    //}
}

