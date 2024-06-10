using SoftwareKingdom.Chess.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChessPlayer 
{
    void OnTurn(ChessState state, int turnIndex);



}
