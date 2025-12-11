using Chess.Engine.Moves;
using Chess.Engine.Pieces;

namespace Chess.Engine;

internal interface IChessGame
{
    PieceColour ToMove { get; }
    GameStatus Status { get; }
    bool MakeMove(Move move);
}