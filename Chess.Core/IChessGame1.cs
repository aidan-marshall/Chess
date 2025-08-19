using Chess.Core.Pieces;

namespace Chess.Core
{
    public interface IChessGame1
    {
        GameStatus Status { get; }
        PieceColour ToMove { get; }
    }
}