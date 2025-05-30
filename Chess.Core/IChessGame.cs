using Chess.Core.Pieces;

namespace Chess.Core;

public interface IChessGame
{
    PieceColour ToMove { get; }
    bool MovePiece(ChessMove move);
}