using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine;

internal interface IChessGame
{
    PieceColour ToMove { get; }
    GameState State { get; }
    IReadOnlyList<Move> MoveHistory { get; }
    GameMoveResult TryMakeMove(Move move, PieceColour moveColour);
}