namespace Chess.Engine;

public enum MoveResult
{
    Success,
    IllegalMove,
    PlayerOutOfTurn,
    PieceNotFound,
    MoveLeavesKingInCheck,
    GameOver,
    PawnPromotion
}
