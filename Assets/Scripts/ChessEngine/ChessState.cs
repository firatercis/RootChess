using SoftwareKingdom.Chess.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoftwareKingdom.Chess.Core
{

    public struct ChessPiece
    {
        byte color; // 0: white 1: black
        byte pieceNotation; //TODO: KN
    }


    public class ChessState
    {
        public const string EMPTY_SQUARE = null;
        public const int EMPTY = -1;
        public const int WHITE = 0;
        public const int BLACK = 1;
        public const char WHITE_PREFIX = 'W';
        public const char BLACK_PREFIX = 'B';
        public const int PIECE_NOTATION_INDEX = 1;
        public const int N_SIDES = 2;
        public static Coord NO_EN_PASSANT_COORD = new Coord(-1,-1);



        // Attributes
        public string[,] board; 
        public List<string> flags;
        public Coord enPassantCoord;
        public int turn; // 0: white, 1:black
        public ChessState(int width, int height)
        {
            board = new string[height, width];
            flags = new List<string>();
            turn = 0;
            enPassantCoord = NO_EN_PASSANT_COORD;  // Unable the En Passant move
        }

        public ChessState(string[,] board, List<string> flags) // TODO: Do not copy the flags string list
        {
            this.board = board;
            this.flags = flags;
            turn = 0;
        }

        public ChessState(ChessState boardState) // Copy constructor, can it be static?
        {
            board = new string[boardState.board.GetLength(0), boardState.board.GetLength(1)];
            for(int i=0; i<boardState.board.GetLength(0); i++)
            {
                for (int j = 0; j < boardState.board.GetLength(1); j++)
                {
                    board[i, j] = boardState.board[i, j];
                }
            }
            flags = new List<string>();
            if(boardState.flags != null)
                flags.AddRange(boardState.flags.ToArray());
            turn = boardState.turn;
        }

   

        public bool IsEmpty(int i, int j)
        {
            return board[i, j] == EMPTY_SQUARE;
        }

        public virtual bool IsInTurn(string piece)
        {
            if (piece == EMPTY_SQUARE) return false;
            bool result = false;
            if (turn == 0 && piece[0] == WHITE_PREFIX   // White's turn
                || turn == 1 && piece[0] == BLACK_PREFIX) // Black's turn
            {
                result = true;
            }
            return result;
            
        }

        public bool IsEmpty(Coord coord)
        {
            if (!IsInsideBoard(coord)) return false;
            return board[coord.rankIndex, coord.fileIndex] == EMPTY_SQUARE;
        }

        public bool IsInsideBoard(Coord coord)
        {
            return coord.rankIndex >= 0 && coord.rankIndex < board.GetLength(0)
                && coord.fileIndex >= 0 && coord.fileIndex < board.GetLength(1);
        }


        public virtual char GetPieceNotation(Coord coord)
        {
            
            string piece = board[coord.rankIndex, coord.fileIndex];
            if (piece == null) return '\0';
            return piece[PIECE_NOTATION_INDEX];
        }

        public virtual int GetColor(string piece)
        {
            if (piece == EMPTY_SQUARE) return EMPTY;
            return piece[0] == WHITE_PREFIX ? WHITE : BLACK;
        }

        public virtual bool CanLandOn(string piece, string target)
        {

            bool result = target == EMPTY_SQUARE ||
                GetColor(piece) != GetColor(target);
            return result;
        }

        public virtual bool CanWalkOver(string piece, string target)
        {
            return target == EMPTY_SQUARE;
        }

        public virtual bool IsEnemy(string piece, string target)
        {

            bool result = target != EMPTY_SQUARE &&
               GetColor(piece) != GetColor(target);
            return result;
        }


        public Coord Search(string pieceNotation)
        {
            Coord result = new Coord(-1, -1); // When coord could not be found TODO: Magic number
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i,j] == null)
                    {
                        continue;
                    }

                    if (board[i, j].Equals(pieceNotation))
                    {

                        result = new Coord(i, j);
                        return result;
                    }
                    
                }
          
            }
            return result;
        }

        // Found from https://stackoverflow.com/questions/287928/how-do-i-overload-the-square-bracket-operator-in-c

        

        public string this[Coord coord]
        {
            get {
                if (IsInsideBoard(coord))
                {
                    return board[coord.rankIndex, coord.fileIndex]; 
                }else{
                    return null;
                }
            }set {
                if(IsInsideBoard(coord))
                    board[coord.rankIndex, coord.fileIndex] = value; 
            }
        }

       

        public string this[int i, int j]
        {
            get { return board[i, j]; }
            set { board[i, j] = value; }
        }

        public char GetTurnPrefix()
        {
            return turn == WHITE ? WHITE_PREFIX : BLACK_PREFIX;
        }

        public string ToString()
        {
            string text = "";
            for(int i=0; i< board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] != EMPTY_SQUARE)
                        text += board[i, j] + "";
                    else
                        text += "__";
                }
                text += "\n";
            }
            text += "" + turn;
            return text;
           
        }

        public override bool Equals(object obj) {
            bool result = true;
            ChessState otherState = (ChessState)obj;

            if(otherState.turn != turn)
                result = false;
            else
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] != otherState.board[i, j])
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            // TODO: flags

            return result;
        }

        public string GetPieceAt(int row, int col)
        {
            return board[row, col];
        }

       
    }
}

