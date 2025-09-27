using Chess.Core.Pieces;

namespace Chess.Core;

public interface IChessGame
{
    PieceColour ToMove { get; }
    GameStatus Status { get; }
    public MoveResult MakeMove(Move move);
}