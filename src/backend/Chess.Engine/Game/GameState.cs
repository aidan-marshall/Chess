namespace Chess.Engine.Game;

public enum GameState
{
    Ongoing,
    Check,
    Checkmate,
    Stalemate,
    Draw,
    PromotionPending,
    Resigned
}
