using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine.Game;

internal interface IChessGame
{
    PieceColour ToMove { get; }
    GameState State { get; }
    IReadOnlyList<Move> MoveHistory { get; }
    GameMoveResult TryMakeMove(Move move, PieceColour moveColour);
    GameMoveResult CompletePromotion(PieceType promotionPieceType);
    string ToFen();
    bool LoadFromFen(string fen, out string? error);
}