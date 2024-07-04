using System;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftwareKingdom.Chess.UI
{

    [CreateAssetMenu(fileName = "AIPlayer", menuName = "SoftwareKingdom/Chess/AIPlayer", order = 0)]

    public class AIPlayer : ChessPlayer 
    {

        // Settings

        // Connections
        public ChessLogic gameLogic;
        ChessState currentChessState;
    
        public override bool IsHuman() { return false; }

        public override void Configure(ChessLogic gameLogic){
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {
            
            
            List<Move> possibleMoves = gameLogic.GenerateBoardMoves(state);
            //Move move = possibleMoves[0];

            if(possibleMoves.Count != 0){
                int randomMoveNumber = GenerateRandomNumber(possibleMoves.Count);
                Move move = possibleMoves[randomMoveNumber];

                gameLogic.PlayMove(state, move);
                
            }
        }

        private int GenerateRandomNumber(int numberOfMoves){

            System.Random random = new System.Random();

            // Üretmek istediğiniz sayı aralığını belirleyin
            int maxNumber = numberOfMoves; 

            // Rastgele bir sayı üretin (0 dahil, maxNumber hariç)
            int randomNumber = random.Next(0, maxNumber);

            return randomNumber;
        }
    }
}


