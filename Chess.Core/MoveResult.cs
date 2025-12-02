namespace Chess.Core;

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
