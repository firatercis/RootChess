using SoftwareKingdom.Chess;
using SoftwareKingdom.Chess.Core;
using SoftwareKingdom.Chess.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPlayer : ScriptableObject
{
    public virtual bool IsHuman() { return false; }
    public abstract void Configure(ChessLogic logic);

    public abstract void OnTurn(ChessState state);
}
