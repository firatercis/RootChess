using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.RootChess;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SoftwareKingdom.Chess.UI
{

    [CreateAssetMenu(fileName = "HumanPlayer", menuName = "SoftwareKingdom/Chess/HumanPlayer", order = 1)]

    public class HumanPlayer : ChessPlayer 
    {

        // Settings

        // Connections
        public ChessLogic gameLogic;

    
        public override bool IsHuman() { return true; }

        public override void Configure(ChessLogic gameLogic)
        {
            this.gameLogic = gameLogic;  
        }

        public override void OnTurn(ChessState state) {
            // Human player is handled by ui,
        }
    }
}


